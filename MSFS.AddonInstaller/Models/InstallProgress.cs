namespace MSFS.AddonInstaller.Models
{
    public class InstallProgress
    {
        public long TotalBytes { get; init; }
        public long CopiedBytes { get; set; }
        public TimeSpan Elapsed { get; set; }

        public double Percentage =>
            TotalBytes == 0 ? 0 : (double)CopiedBytes / TotalBytes * 100;

        public TimeSpan Eta
        {
            get
            {
                if (CopiedBytes == 0)
                    return TimeSpan.Zero;

                var bytesPerSecond = CopiedBytes / Elapsed.TotalSeconds;
                var remainingBytes = TotalBytes - CopiedBytes;

                return TimeSpan.FromSeconds(remainingBytes / bytesPerSecond);
            }
        }
    }
}
