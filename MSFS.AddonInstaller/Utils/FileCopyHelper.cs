using System.Collections.Generic;

namespace MSFS.AddonInstaller.Utils
{
    public static class FileCopyHelper
    {
        public static void CopyDirectory(
            string sourceDir,
            string targetDir)
        {
            foreach (var file in EnumerateFiles(sourceDir))
            {
                var relativePath =
                    Path.GetRelativePath(sourceDir, file);

                var destinationFile =
                    Path.Combine(targetDir, relativePath);

                Directory.CreateDirectory(
                    Path.GetDirectoryName(destinationFile)!
                );

                using var sourceStream = new FileStream(
                    file,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.Read,
                    bufferSize: 1024 * 1024,
                    FileOptions.SequentialScan
                );

                using var destinationStream = new FileStream(
                    destinationFile,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize: 1024 * 1024
                );

                sourceStream.CopyTo(destinationStream);
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
