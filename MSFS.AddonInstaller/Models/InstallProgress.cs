namespace MSFS.AddonInstaller.Models
{
    public class InstallProgress
    {
        public long TotalBytes { get; set; }
        public long CopiedBytes { get; set; }
        public TimeSpan Elapsed { get; set; }

    }
}
