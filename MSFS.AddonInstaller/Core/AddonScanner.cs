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
                        IsDirectory = true,
                        Extension = string.Empty
                    });
                }
                else if (File.Exists(path))
                {
                    addons.Add(new Addon
                    {
                        Name = Path.GetFileNameWithoutExtension(path),
                        SourcePath = path,
                        IsDirectory = false,
                        Extension = Path.GetExtension(path)
                    });
                }
            }

            if (addons.Count == 0)
                throw new InvalidOperationException(
                    "No se pudieron procesar los elementos proporcionados."
                );

            return addons;
        }
    }
}
