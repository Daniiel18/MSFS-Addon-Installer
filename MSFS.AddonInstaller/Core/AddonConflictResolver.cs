using MSFS.AddonInstaller.Models;
using MSFS.AddonInstaller.Utils;

namespace MSFS.AddonInstaller.Core
{
    public static class AddonConflictResolver
    {
        public static IReadOnlyList<AddonRootInstallState> GetInstallStates(
            Addon addon,
            string communityPath)
        {
            var results = new List<AddonRootInstallState>();

            string inspectionPath = addon.IsDirectory
                ? addon.SourcePath
                : ArchiveExtractor.ExtractToTemp(addon.SourcePath);

            try
            {
                var addonRoots =
                    AddonContentResolver.ResolveAddonRoots(inspectionPath);

                foreach (var root in addonRoots)
                {
                    var rootName = Path.GetFileName(root);
                    var targetPath = Path.Combine(communityPath, rootName);

                    var state = Directory.Exists(targetPath)
                        ? AddonInstallState.AlreadyInstalled
                        : AddonInstallState.NotInstalled;

                    results.Add(new AddonRootInstallState
                    {
                        RootName = rootName,
                        SourcePath = root,
                        State = state
                    });
                }
            }
            finally
            {
                if (!addon.IsDirectory && Directory.Exists(inspectionPath))
                {
                    Directory.Delete(inspectionPath, true);
                }
            }

            return results;
        }
    }
}
