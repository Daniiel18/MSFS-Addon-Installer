using MSFS.AddonInstaller.Models;
using MSFS.AddonInstaller.Utils;

namespace MSFS.AddonInstaller.Core
{
    public static class AddonInstaller
    {
        public static void Install(Addon addon, string communityPath)
        {
            string sourcePath = addon.SourcePath;
            string tempPath = string.Empty;

            if (!addon.IsDirectory)
            {
                tempPath = ArchiveExtractor.ExtractToTemp(addon.SourcePath);
                sourcePath = AddonContentResolver.ResolveAddonRoot(tempPath);
            }

            addon.Type = AddonTypeDetector.Detect(sourcePath);

            var targetPath = Path.Combine(communityPath, Path.GetFileName(sourcePath));

            Console.WriteLine($"   {addon.Type}");

            FileCopyHelper.CopyDirectory(
                sourcePath,
                targetPath,
                DrawProgressBar
            );

            if (!string.IsNullOrEmpty(tempPath))
            {
                Directory.Delete(tempPath, true);
            }

            Console.WriteLine();
            Console.WriteLine("Installation completed.");
            Console.WriteLine();
        }

        private static void DrawProgressBar(InstallProgress progress)
        {
            const int barWidth = 25;

            var filledBars = (int)(progress.Percentage / 100 * barWidth);
            var bar = new string('#', filledBars).PadRight(barWidth, '-');

            Console.CursorLeft = 3;
            Console.Write(
                $"{bar} {progress.Percentage:0}% ETA: {progress.Eta:mm\\:ss}"
            );
        }
    }
}
