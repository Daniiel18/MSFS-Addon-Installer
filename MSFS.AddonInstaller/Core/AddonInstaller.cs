using MSFS.AddonInstaller.Models;
using MSFS.AddonInstaller.Utils;

namespace MSFS.AddonInstaller.Core
{
    public static class AddonInstaller
    {
        public static IReadOnlyList<InstallResult> Install(
            Addon addon,
            string communityPath)
        {
            var results = new List<InstallResult>();

            // ============================
            // CASE 1: ARCHIVE
            // ============================
            if (addon.IsArchive)
            {
                var extractedRoot =
                    ArchiveExtractor.ExtractToTemp(addon.SourcePath);

                var addonRoots =
                    AddonContentResolver.ResolveAddonRoots(extractedRoot);

                if (addonRoots.Count == 0)
                {
                    Logger.Info($"No addon roots found in archive '{addon.Name}'");
                }

                if (addonRoots.Count > 1)
                {
                    DrawAttentionBlock(addon.Name, addonRoots);
                }

                foreach (var root in addonRoots)
                {
                    var result = InstallSingleAddon(root, communityPath);
                    results.Add(result);
                }

                Directory.Delete(extractedRoot, true);
                return results;
            }

            // ============================
            // CASE 2: DIRECTORY
            // ============================
            var roots =
                AddonContentResolver.ResolveAddonRoots(addon.SourcePath);

            if (roots.Count > 1)
            {
                DrawAttentionBlock(addon.Name, roots);
            }

            foreach (var root in roots)
            {
                var result = InstallSingleAddon(root, communityPath);
                results.Add(result);
            }

            return results;
        }

        // ==================================================
        // INSTALL A SINGLE ADDON ROOT
        // ==================================================
        private static InstallResult InstallSingleAddon(
            string sourcePath,
            string communityPath)
        {
            bool isUpdate = false;

            var addonName = Path.GetFileName(sourcePath);
            var targetPath = Path.Combine(communityPath, addonName);

            // ----------------------------
            // EXISTING ADDON
            // ----------------------------
            if (Directory.Exists(targetPath))
            {
                var comparison = AddonComparer.Compare(sourcePath, targetPath);

                switch (comparison)
                {
                    case AddonComparisonResult.SameVersion:
                        Logger.Info($"Skipped '{addonName}' (same version)");
                        return new InstallResult
                        {
                            Name = addonName,
                            Status = InstallStatus.SkippedSameVersion,
                            SizeBytes = 0,
                            Duration = TimeSpan.Zero
                        };

                    case AddonComparisonResult.Downgrade:
                        Logger.Info($"Skipped '{addonName}' (installed version is newer)");
                        return new InstallResult
                        {
                            Name = addonName,
                            Status = InstallStatus.SkippedNewerInstalled,
                            SizeBytes = 0,
                            Duration = TimeSpan.Zero
                        };

                    case AddonComparisonResult.UpdateAvailable:
                        Logger.Info($"Updating '{addonName}' to newer version");
                        Directory.Delete(targetPath, true);
                        isUpdate = true;
                        break;

                    case AddonComparisonResult.DifferentAddon:
                        throw new InvalidOperationException(
                            $"Addon '{addonName}' conflicts with existing content."
                        );
                }
            }

            // ----------------------------
            // INSTALL / UPDATE
            // ----------------------------
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            FileCopyHelper.CopyDirectory(sourcePath, targetPath);

            stopwatch.Stop();

            long sizeBytes = Directory
                .EnumerateFiles(targetPath, "*", SearchOption.AllDirectories)
                .Sum(f => new FileInfo(f).Length);

            return new InstallResult
            {
                Name = addonName,
                Status = isUpdate
        ? InstallStatus.Updated
        : InstallStatus.Installed,
                SizeBytes = sizeBytes,
                Duration = stopwatch.Elapsed
            };
        }

        // ==================================================
        // UI HELPERS
        // ==================================================
        private static void DrawAttentionBlock(
            string containerName,
            IReadOnlyList<string> addonRoots)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            DrawInlineHeader(
                $"ATTENTION! Multiple add-ons detected in: {containerName}"
            );

            Console.WriteLine();

            var names = addonRoots
            .Select(Path.GetFileName)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(n => n!) // <- afirmamos null-safety aquí
            .ToList();

            WriteEnumeratedColumns(names);

            Console.ResetColor();
            Console.WriteLine();
        }

        private static void WriteEnumeratedColumns(
            IReadOnlyList<string> items,
            int padding = 6)
        {
            if (items.Count == 0)
                return;

            int consoleWidth = Console.WindowWidth - 1;

            var numberedItems = items
                .Select((item, index) => $"{index + 1}. {item}")
                .ToList();

            int maxItemLength = numberedItems.Max(i => i.Length) + padding;
            int columns = Math.Max(1, consoleWidth / maxItemLength);
            int rows = (int)Math.Ceiling(numberedItems.Count / (double)columns);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    int index = r + (c * rows);
                    if (index >= numberedItems.Count)
                        continue;

                    Console.Write(
                        numberedItems[index].PadRight(maxItemLength)
                    );
                }
                Console.WriteLine();
            }
        }

        private static void DrawInlineHeader(string text)
        {
            int width = Console.WindowWidth - 1;

            var line = $"── {text} ";

            if (line.Length < width)
                line += new string('─', width - line.Length);

            Console.WriteLine(line);
        }
    }
}