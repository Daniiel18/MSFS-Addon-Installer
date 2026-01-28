namespace MSFS.AddonInstaller.Utils
{
    public static class AddonContentResolver
    {
        public static string ResolveAddonRoot(string extractedPath)
        {
            var directories = Directory.GetDirectories(extractedPath);

            // Caso común: un solo folder en la raíz
            if (directories.Length == 1 && !HasAddonFiles(extractedPath))
                return directories[0];

            return extractedPath;
        }

        private static bool HasAddonFiles(string path)
        {
            return File.Exists(Path.Combine(path, "layout.json")) ||
                   File.Exists(Path.Combine(path, "manifest.json"));
        }
    }
}
