namespace MSFS.AddonInstaller.Models
{
    public class Addon
    {
        public string Name { get; init; } = string.Empty;
        public string SourcePath { get; init; } = string.Empty;
        public bool IsDirectory { get; init; }
        public string Extension { get; init; } = string.Empty;
    }
}
