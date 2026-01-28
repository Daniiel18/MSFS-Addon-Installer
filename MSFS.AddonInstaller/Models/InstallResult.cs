namespace MSFS.AddonInstaller.Models
{
    public class InstallResult
    {
        public string Name { get; init; } = string.Empty;
        public long SizeBytes { get; init; }
        public TimeSpan Duration { get; init; }
    }
}
