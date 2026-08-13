using System.Collections.Generic;

namespace Tomat.FNB.TMOD;

/// <summary>
///     A simple read-only <c>.tmod</c> file implementation that wraps an
///     existing instance.
/// </summary>
public readonly struct ReadOnlyTmodFile(IReadOnlyTmodFile tmod) : IReadOnlyTmodFile
{
    string IReadOnlyTmodFile.ModLoaderVersion => tmod.ModLoaderVersion;

    string IReadOnlyTmodFile.Name => tmod.Name;

    string IReadOnlyTmodFile.Version => tmod.Version;

    IReadOnlyList<(string path, byte[] data)> IReadOnlyTmodFile.Entries => tmod.Entries;

    byte[] IReadOnlyTmodFile.this[string path] => tmod[path];
}