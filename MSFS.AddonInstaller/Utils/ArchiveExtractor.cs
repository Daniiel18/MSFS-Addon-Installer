using MSFS.AddonInstaller.Models;
using SharpCompress.Archives;
using System;

namespace MSFS.AddonInstaller.Utils
{
    public static class ArchiveExtractor
    {
        // ==================================================
        // SLOW PATH → 7Z / RAR
        // ==================================================
        public static string ExtractToTemp(string archivePath)
        {
            var tempDir = Path.Combine(
                Path.GetTempPath(),
                "MSFS_AddonInstaller",
                Guid.NewGuid().ToString()
            );

            Directory.CreateDirectory(tempDir);

            using var archive = ArchiveFactory.Open(archivePath);

            foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
            {
                entry.WriteToDirectory(
                    tempDir,
                    new SharpCompress.Common.ExtractionOptions
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    }
                );
            }

            return tempDir;
        }
    }
}
