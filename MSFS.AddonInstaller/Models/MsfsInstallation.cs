namespace MSFS.AddonInstaller.Models
{
    public class MsfsInstallation
    {
        public string SimulatorVersion { get; init; } = string.Empty;
        public string InstalledPackagesPath { get; init; } = string.Empty;
        public string CommunityPath =>
            Path.Combine(InstalledPackagesPath, "Community");
    }
}
