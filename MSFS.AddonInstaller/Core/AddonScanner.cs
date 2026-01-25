using MSFS.AddonInstaller.Models;

namespace MSFS.AddonInstaller.Core
{
    public static class AddonScanner
    {
        public static IReadOnlyList<Addon> Scan(string[] args)
        {
            if (args.Length == 0)
                throw new ArgumentException(
                    "No se detectaron addons. Arrastra uno o más archivos o carpetas al ejecutable."
                );

            var addons = new List<Addon>();

            foreach (var path in args)
            {
                if (Directory.Exists(path))
                {
                    addons.Add(new Addon
                    {
                        Name = Path.GetFileName(path),
                        SourcePath = path,
                        IsZip = false
                    });
                }
                else if (File.Exists(path) && Path.GetExtension(path).Equals(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    addons.Add(new Addon
                    {
                        Name = Path.GetFileNameWithoutExtension(path),
                        SourcePath = path,
                        IsZip = true
                    });
                }
            }

            if (addons.Count == 0)
                throw new InvalidOperationException(
                    "Los archivos proporcionados no son addons válidos."
                );

            return addons;
        }
    }
}
