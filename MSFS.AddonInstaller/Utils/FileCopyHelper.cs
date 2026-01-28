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
            var progress = new InstallProgress
            {
                TotalBytes = 0,
                CopiedBytes = 0,
                Elapsed = TimeSpan.Zero
            };

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            foreach (var file in EnumerateFiles(sourceDir))
            {
                var fileInfo = new FileInfo(file);
                progress.TotalBytes += fileInfo.Length;

                var relativePath = Path.GetRelativePath(sourceDir, file);
                var destinationFile = Path.Combine(targetDir, relativePath);

                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile)!);

                using var sourceStream = new FileStream(
                    file,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    1024 * 1024,
                    FileOptions.SequentialScan
                );

                using var destinationStream = new FileStream(
                    destinationFile,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    1024 * 1024
                );

                var buffer = new byte[1024 * 1024];
                int bytesRead;

                while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    destinationStream.Write(buffer, 0, bytesRead);

                    progress.CopiedBytes += bytesRead;
                    progress.Elapsed = stopwatch.Elapsed;

                    progressCallback(progress);
                }
            }
        }

        private static IEnumerable<string> EnumerateFiles(string root)
        {
            var stack = new Stack<string>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                var dir = stack.Pop();

                foreach (var subDir in Directory.GetDirectories(dir))
                    stack.Push(subDir);

                foreach (var file in Directory.GetFiles(dir))
                    yield return file;
            }
        }
    }
}
