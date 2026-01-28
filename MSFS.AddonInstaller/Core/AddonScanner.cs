using MSFS.AddonInstaller.Models;
using SharpCompress.Archives;

namespace MSFS.AddonInstaller.Core
{
    public static class AddonScanner
    {
        private static readonly HashSet<string> SupportedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
        ".zip",
        ".rar"
        };

        private static readonly HashSet<string> RequiresExtraction =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ".7z",
                ".rar"
            };

        public static IReadOnlyList<Addon> Scan(string[] args)
        {
            var addons = new List<Addon>();

            foreach (var path in args)
            {
                if (Directory.Exists(path))
                {
                    addons.Add(new Addon
                    {
                        Name = Path.GetFileName(path),
                        SourcePath = path,
                        IsDirectory = true,
                        IsArchive = false,
                        Extension = string.Empty
                    });

                    continue;
                }

                if (!File.Exists(path))
                    continue;

                var extension = Path.GetExtension(path);

                addons.Add(new Addon
                {
                    Name = Path.GetFileName(path),
                    SourcePath = path,
                    IsDirectory = false,
                    IsArchive = SupportedExtensions.Contains(extension),
                    Extension = extension
                });
            }

            return addons;
        }
        public static IReadOnlyList<string> ScanExtractedRoots(string extractedPath)
        {
            return Directory.GetDirectories(extractedPath, "*", SearchOption.AllDirectories)
                .Where(d =>
                    File.Exists(Path.Combine(d, "manifest.json")) &&
                    File.Exists(Path.Combine(d, "layout.json"))
                )
                .ToList();
        }
    }
}