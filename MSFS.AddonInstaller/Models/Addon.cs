namespace MSFS.AddonInstaller.Models
{
    public sealed class Addon
    {
        public string SourcePath { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;

        public bool IsDirectory { get; init; }
        public bool IsArchive { get; init; }

        public string Extension { get; init; } = string.Empty;

    }
}
