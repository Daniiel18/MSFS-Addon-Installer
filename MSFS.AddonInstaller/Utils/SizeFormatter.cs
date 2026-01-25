namespace MSFS.AddonInstaller.Utils
{
    public static class SizeFormatter
    {
        public static string Format(long bytes)
        {
            const double KB = 1024.0;
            const double MB = KB * 1024;
            const double GB = MB * 1024;

            if (bytes >= GB)
                return $"{bytes / GB:0.00} GB";

            if (bytes >= MB)
                return $"{bytes / MB:0.00} MB";

            if (bytes >= KB)
                return $"{bytes / KB:0.00} KB";

            return $"{bytes} B";
        }
    }
}
