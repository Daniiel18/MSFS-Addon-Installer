using MSFS.AddonInstaller.Models;
using SharpCompress.Archives;
using System;

namespace MSFS.AddonInstaller.Utils
{
    public static class ArchiveExtractor
    {
        public static InstallResult InstallFromArchiveStreaming(
            string archivePath,
            string communityPath,
            Action<InstallProgress> progressCallback)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            using var archive = ArchiveFactory.Open(archivePath);

            var manifestEntry = archive.Entries
                .FirstOrDefault(e =>
                    !e.IsDirectory &&
                    e.Key.EndsWith("manifest.json", StringComparison.OrdinalIgnoreCase));

            if (manifestEntry == null)
                throw new InvalidOperationException("manifest.json not found.");

            var manifestPath = manifestEntry.Key.Replace('\\', '/');
            var addonRootPath = manifestPath[..manifestPath.LastIndexOf('/')];

            var addonName = Path.GetFileName(addonRootPath);
            var finalDestination = Path.Combine(communityPath, addonName);

            if (Directory.Exists(finalDestination))
                throw new InvalidOperationException("Addon already installed.");

            Directory.CreateDirectory(finalDestination);

            long totalBytes = archive.Entries
                .Where(e => !e.IsDirectory)
                .Sum(e => (long)e.Size);

            long copiedBytes = 0;

            var progress = new InstallProgress
            {
                TotalBytes = totalBytes
            };

            foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
            {
                var normalized = entry.Key.Replace('\\', '/');

                if (!normalized.StartsWith(addonRootPath))
                    continue;

                var relativePath = normalized
                    .Substring(addonRootPath.Length)
                    .TrimStart('/');

                var destinationPath = Path.Combine(finalDestination, relativePath);

                var dir = Path.GetDirectoryName(destinationPath);
                if (dir != null)
                    Directory.CreateDirectory(dir);

                using var entryStream = entry.OpenEntryStream();
                using var fileStream = File.Create(destinationPath);

                entryStream.CopyTo(fileStream);

                copiedBytes += (long)entry.Size;

                progress.CopiedBytes = copiedBytes;
                progress.Elapsed = stopwatch.Elapsed;

                progressCallback(progress);
            }

            stopwatch.Stop();

            return new InstallResult
            {
                Name = addonName,
                SizeBytes = totalBytes,
                Duration = stopwatch.Elapsed
            };
        }
    }
}
