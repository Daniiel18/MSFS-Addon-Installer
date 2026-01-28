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
            InstallProgress? lastProgress = null;

            // ============================
            // CASE 1: ARCHIVE (STREAMING)
            // ============================
            if (!addon.IsDirectory)
            {
                Console.WriteLine("   Installing...");

                var result = ArchiveExtractor.InstallFromArchiveStreaming(
                    addon.SourcePath,
                    communityPath,
                    progress =>
                    {
                        lastProgress = progress;
                        DrawProgressBar(progress);
                    }
                );

                Console.WriteLine();
                Console.WriteLine(
                    $"Installation completed in {result.Duration:mm\\:ss}"
                );
                Console.WriteLine();

                return result;
            }

            // ============================
            // CASE 2: DIRECTORY
            // ============================
            addon.Type = AddonTypeDetector.Detect(sourcePath);

            var targetPath = Path.Combine(
                communityPath,
                Path.GetFileName(sourcePath)
            );

            if (Directory.Exists(targetPath))
            {
                Console.WriteLine("   Already installed - skipped.");
                return null;
            }

            Console.WriteLine($"   {addon.Type}");

            FileCopyHelper.CopyDirectory(
                sourcePath,
                targetPath,
                progress =>
                {
                    lastProgress = progress;
                    DrawProgressBar(progress);
                }
            );

            addonStopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine(
                $"Installation completed in {addonStopwatch.Elapsed:mm\\:ss}"
            );
            Console.WriteLine();

            return new InstallResult
            {
                Name = Path.GetFileName(sourcePath),
                SizeBytes = lastProgress?.TotalBytes ?? 0,
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
