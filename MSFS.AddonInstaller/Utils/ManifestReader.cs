using System.Text.Json;

namespace MSFS.AddonInstaller.Utils
{
    public sealed class ManifestData
    {
        public string PackageName { get; init; } = string.Empty;
        public string PackageVersion { get; init; } = "0.0.0";
    }

    public static class ManifestReader
    {
        public static ManifestData Read(string addonRootPath)
        {
            var manifestPath = Path.Combine(addonRootPath, "manifest.json");

            if (!File.Exists(manifestPath))
                throw new InvalidOperationException(
                    $"manifest.json not found in '{addonRootPath}'"
                );

            using var stream = File.OpenRead(manifestPath);
            using var document = JsonDocument.Parse(stream);

            var root = document.RootElement;

            string packageName = root.TryGetProperty("package_name", out var nameProp)
                ? nameProp.GetString() ?? string.Empty
                : string.Empty;

            string packageVersion = root.TryGetProperty("package_version", out var versionProp)
                ? versionProp.GetString() ?? "0.0.0"
                : "0.0.0";

            return new ManifestData
            {
                PackageName = packageName,
                PackageVersion = packageVersion
            };
        }
    }
}
