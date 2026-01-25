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

            var targetPath = Path.Combine(communityPath, Path.GetFileName(sourcePath));

            Console.WriteLine($"Instalando: {Path.GetFileName(sourcePath)}");

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
            Console.WriteLine("Instalación completada.");
            Console.WriteLine();
        }

        private static void DrawProgressBar(InstallProgress progress)
        {
            const int barWidth = 30;

            var filledBars = (int)(progress.Percentage / 100 * barWidth);
            var bar = new string('#', filledBars).PadRight(barWidth, '-');

            Console.CursorLeft = 0;
            Console.Write(
                $"[{bar}] {progress.Percentage:0.0}% ETA: {progress.Eta:mm\\:ss}"
            );
        }
    }
}
