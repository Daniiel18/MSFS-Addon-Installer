namespace MSFS.AddonInstaller.Models
{
    public class InstallProgress
    {
        public long TotalBytes { get; set; }
        public long CopiedBytes { get; set; }
        public TimeSpan Elapsed { get; set; }

        public double Percentage =>
            TotalBytes == 0 ? 0 : (double)CopiedBytes / TotalBytes * 100;
    }
}
