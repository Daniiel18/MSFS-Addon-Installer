using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MSFS.AddonInstaller.Core
{
    public static class AddonContentResolver
    {
        public static IReadOnlyList<string> ResolveAddonRoots(string rootPath)
        {
            var results = new List<string>();

            foreach (var dir in Directory.EnumerateDirectories(
                rootPath,
                "*",
                SearchOption.AllDirectories))
            {
                var manifest = Path.Combine(dir, "manifest.json");
                var layout = Path.Combine(dir, "layout.json");

                if (File.Exists(manifest) && File.Exists(layout))
                {
                    results.Add(dir);
                }
            }

            return results;
        }
    }
}
