using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks.Dataflow;

using Tomat.FNB.Common.Compression;
using Tomat.FNB.TMOD.Converters;

namespace Tomat.FNB.TMOD.Utilities;

public static class TmodExtensions
{
    /// <summary>
    ///     Creates a read-only view into a <c>.tmod</c> file.
    /// </summary>
    /// <param name="tmod">The <c>.tmod</c> file to wrap.</param>
    /// <returns>A read-only view into the <c>.tmod</c> file.</returns>
    public static ReadOnlyTmodFile AsReadOnly(
        this IReadOnlyTmodFile tmod
    )
    {
        return new ReadOnlyTmodFile(tmod);
    }

    public static byte[] Decompress(byte[] data, int uncompressedLength)
    {
        // In cases where the file isn't actually compressed.  This is possible
        // with custom fnb settings or if a tModLoader `.tmod` contains files
        // small enough to not be compressed.
        if (data.Length == uncompressedLength)
        {
            return data;
        }

        // TODO(perf): Benchmark again to confirm this is worthwhile.
        //             The garbage collector already tries to zero-initialize
        //             ahead of time but we can end up allocating rather large
        //             arrays...
        // We can skip zero-initialization because we overwrite all the data in
        // the allocated array regardless.
        var array = GC.AllocateUninitializedArray<byte>(uncompressedLength);

        using var ds = new DeflateDecompressor();
        {
            ds.Decompress(data, new Span<byte>(array), out var written);
            {
                Debug.Assert(written == uncompressedLength && array.Length == uncompressedLength);
            }
        }

        return array;
    }

    private static byte[] Compress(byte[] data)
    {
        using var ms = new MemoryStream(data);
        using (var ds = new DeflateStream(ms, CompressionMode.Compress))
        {
            ds.Write(data, 0, data.Length);
        }

        // TODO(perf): Prefer GetBuffer?
        var compressedData = ms.ToArray();
        return compressedData;
    }
}