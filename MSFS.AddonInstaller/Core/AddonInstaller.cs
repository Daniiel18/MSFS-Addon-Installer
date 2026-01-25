using MSFS.AddonInstaller.Models;
using MSFS.AddonInstaller.Utils;

namespace MSFS.AddonInstaller.Core
{
    public static class AddonInstaller
    {
        public static InstallResult? Install(Addon addon, string communityPath)
        {
            var addonStopwatch = System.Diagnostics.Stopwatch.StartNew();

            string sourcePath = addon.SourcePath;
            string tempPath = string.Empty;

            // Extract archive if needed
            if (!addon.IsDirectory)
            {
                tempPath = ArchiveExtractor.ExtractToTemp(addon.SourcePath);
                sourcePath = AddonContentResolver.ResolveAddonRoot(tempPath);
            }

            // Detect addon type
            addon.Type = AddonTypeDetector.Detect(sourcePath);

            var targetPath = Path.Combine(
                communityPath,
                Path.GetFileName(sourcePath)
            );

            // Skip if already installed
            if (Directory.Exists(targetPath))
            {
                Console.WriteLine("   Already installed - skipped.");
                return null;
            }

            Console.WriteLine($"   {addon.Type}");

            InstallProgress? lastProgress = null;

            FileCopyHelper.CopyDirectory(
                sourcePath,
                targetPath,
                progress =>
                {
                    lastProgress = progress;
                    DrawProgressBar(progress);
                }
            );

            if (!string.IsNullOrEmpty(tempPath))
            {
                Directory.Delete(tempPath, true);
            }

            Console.WriteLine();

            if (lastProgress != null)
            {
                Console.WriteLine(
                    $"Installation completed in {lastProgress.Elapsed:mm\\:ss}"
                );
            }
            else
            {
                Console.WriteLine("Installation completed.");
            }

            Console.WriteLine();

            long sizeBytes = Directory
                .EnumerateFiles(targetPath, "*", SearchOption.AllDirectories)
                .Sum(f => new FileInfo(f).Length);

            addonStopwatch.Stop();

            return new InstallResult
            {
                Name = Path.GetFileName(sourcePath),
                SizeBytes = sizeBytes,
                Duration = addonStopwatch.Elapsed
            };
        }

        private static void DrawProgressBar(InstallProgress progress)
        {
            const int barWidth = 25;

            var filledBars =
                progress.TotalBytes == 0
                    ? 0
                    : (int)(progress.Percentage / 100 * barWidth);

            var bar = new string('#', filledBars).PadRight(barWidth, '-');

            Console.CursorLeft = 3;
            Console.Write($"{bar} {progress.Percentage:0}%");
        }
    }
}
