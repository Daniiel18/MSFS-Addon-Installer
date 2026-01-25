namespace MSFS.AddonInstaller.Utils
{
    public static class Logger
    {
        private static string _logFilePath = string.Empty;

        public static void Initialize()
        {
            var appDirectory = AppContext.BaseDirectory;
            _logFilePath = Path.Combine(
                appDirectory,
                "MSFS_AddonInstaller.log"
            );
        }

        public static void Info(string message)
        {
            Write("INFO", message);
        }

        public static void Error(string message)
        {
            Write("ERROR", message);
        }

        public static void Error(Exception ex)
        {
            Write(
                "ERROR",
                $"{ex.Message}{Environment.NewLine}{ex.StackTrace}"
            );
        }

        private static void Write(string level, string message)
        {
            if (string.IsNullOrEmpty(_logFilePath))
                return;

            var line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";

            File.AppendAllText(_logFilePath, line + Environment.NewLine);
        }
    }
}
