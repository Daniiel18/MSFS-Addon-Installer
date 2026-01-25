using MSFS.AddonInstaller.Models;
using System.Text.RegularExpressions;

namespace MSFS.AddonInstaller.Core
{
    public static class AddonTypeDetector
    {
        private static readonly Regex IcaoRegex =
            new Regex(@"\b[A-Z]{4}\b", RegexOptions.IgnoreCase);

        public static AddonType Detect(string addonRoot)
        {
            var simObjectsAircraft =
                Path.Combine(addonRoot, "SimObjects", "Airplanes");

            var sceneryWorldScenery =
                Path.Combine(addonRoot, "scenery", "world", "scenery");

            var modelLib =
                Path.Combine(addonRoot, "modelLib");

            // 1. LIVERY
            if (Directory.Exists(simObjectsAircraft))
            {
                foreach (var aircraft in Directory.GetDirectories(simObjectsAircraft))
                {
                    var textures = Directory.GetDirectories(aircraft, "texture*");
                    var modelCfg = Path.Combine(aircraft, "model.cfg");

                    if (textures.Length > 0 && !File.Exists(modelCfg))
                        return AddonType.Livery;
                }
            }

            // 2. AIRCRAFT
            if (Directory.Exists(simObjectsAircraft))
            {
                foreach (var aircraft in Directory.GetDirectories(simObjectsAircraft))
                {
                    if (File.Exists(Path.Combine(aircraft, "aircraft.cfg")) &&
                        File.Exists(Path.Combine(aircraft, "model.cfg")))
                    {
                        return AddonType.Aircraft;
                    }
                }
            }

            // 3. SCENERY (estructura canónica, O(1))
            if (Directory.Exists(sceneryWorldScenery))
                return AddonType.Scenery;

            // 4. LIBRARY / MOD
            if (Directory.Exists(modelLib) ||
                Directory.Exists(Path.Combine(addonRoot, "scenery")))
                return AddonType.Library;

            // 5. UNKNOWN
            return AddonType.Unknown;
        }
    }
}
