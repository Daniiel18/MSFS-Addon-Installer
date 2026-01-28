using MSFS.AddonInstaller.Models;

namespace MSFS.AddonInstaller.Utils
{
    public static class AddonComparer
    {
        public static AddonComparisonResult Compare(
            string sourceAddonPath,
            string installedAddonPath)
        {
            var sourceManifest = ManifestReader.Read(sourceAddonPath);
            var installedManifest = ManifestReader.Read(installedAddonPath);

            if (!string.Equals(
                sourceManifest.PackageName,
                installedManifest.PackageName,
                StringComparison.OrdinalIgnoreCase))
            {
                return AddonComparisonResult.DifferentAddon;
            }

            var sourceVersion = Version.Parse(sourceManifest.PackageVersion);
            var installedVersion = Version.Parse(installedManifest.PackageVersion);

            if (sourceVersion == installedVersion)
                return AddonComparisonResult.SameVersion;

            if (sourceVersion > installedVersion)
                return AddonComparisonResult.UpdateAvailable;

            return AddonComparisonResult.Downgrade;
        }
    }
}
