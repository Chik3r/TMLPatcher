using System.Collections.Generic;
using System.Linq;

namespace Tomat.FNB.TMOD;

/// <summary>
///     A <c>.tmod</c> file.
/// </summary>
public sealed class TmodFile(
    string                 modLoaderVersion,
    string                 name,
    string                 version,
    List<(string path, byte[] data)> entries
) : ITmodFile, IReadOnlyTmodFile
{
    public string ModLoaderVersion { get; set; } = modLoaderVersion;

    public string Name { get; set; } = name;

    public string Version { get; set; } = version;

    public IList<(string path, byte[] data)> Entries { get; } = entries;

    IReadOnlyList<(string path, byte[] data)> IReadOnlyTmodFile.Entries { get; } = entries;

    public byte[] this[string path] => Entries.First(x => x.path == path).data;

    public void AddFile(string path, byte[] data)
    {
        path = SanitizePath(path);

        entries.Add((path, data));
    }

    public bool RemoveFile(string path)
    {
        path = SanitizePath(path);

        int idx = entries.FindIndex(x => x.path == path);
        if (idx >= 0) {
            entries.RemoveAt(idx);
            return true;
        }
        return false;
    }

    private static string SanitizePath(string path)
    {
        const char dirty_separator = '\\';
        const char clean_separator = '/';

        return path.Trim().Replace(dirty_separator, clean_separator);
    }
}