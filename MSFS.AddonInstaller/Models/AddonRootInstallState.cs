namespace MSFS.AddonInstaller.Models
{
    public sealed class AddonRootInstallState
    {
        public string RootName { get; init; } = string.Empty;
        public string SourcePath { get; init; } = string.Empty;
        public AddonInstallState State { get; init; }
    }
}
