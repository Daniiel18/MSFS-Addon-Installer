using MSFS.AddonInstaller.Models;

namespace MSFS.AddonInstaller.Utils
{
    public static class FileCopyHelper
    {
        public static void CopyDirectory(
            string sourceDir,
            string targetDir,
            Action<InstallProgress> progressCallback)
        {
            var files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);

            long totalBytes = files.Sum(f => new FileInfo(f).Length);
            long copiedBytes = 0;

            var progress = new InstallProgress
            {
                TotalBytes = totalBytes
            };

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(sourceDir, file);
                var destinationFile = Path.Combine(targetDir, relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile)!);

                using var sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read);
                using var destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write);

                var buffer = new byte[1024 * 1024]; // 1 MB
                int bytesRead;

                while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    destinationStream.Write(buffer, 0, bytesRead);
                    copiedBytes += bytesRead;

                    progress.CopiedBytes = copiedBytes;
                    progress.Elapsed = stopwatch.Elapsed;

                    progressCallback(progress);
                }
            }
        }
    }
}
