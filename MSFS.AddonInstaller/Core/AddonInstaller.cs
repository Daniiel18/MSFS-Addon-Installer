using MSFS.AddonInstaller.Models;
using MSFS.AddonInstaller.Utils;

namespace MSFS.AddonInstaller.Core
{
    public static class AddonInstaller
    {
        public static void Install(Addon addon, string communityPath)
        {
            if (!addon.IsDirectory)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Saltando {addon.Name} ({addon.Extension}) – extracción no implementada aún.");
                Console.ResetColor();
                return;
            }

            var targetPath = Path.Combine(communityPath, addon.Name);

            Console.WriteLine($"Instalando: {addon.Name}");

            FileCopyHelper.CopyDirectory(
                addon.SourcePath,
                targetPath,
                progress =>
                {
                    DrawProgressBar(progress);
                });

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
