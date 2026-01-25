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
            var simObjectsPath = Path.Combine(addonRoot, "SimObjects", "Airplanes");
            var sceneryPath = Path.Combine(addonRoot, "scenery");
            var modelLibPath = Path.Combine(addonRoot, "modelLib");

            // 1. LIVERY (highest priority)
            if (Directory.Exists(simObjectsPath))
            {
                foreach (var aircraft in Directory.GetDirectories(simObjectsPath))
                {
                    var textures = Directory.GetDirectories(aircraft, "texture*", SearchOption.TopDirectoryOnly);
                    var modelCfg = Path.Combine(aircraft, "model.cfg");

                    if (textures.Any() && !File.Exists(modelCfg))
                        return AddonType.Livery;
                }
            }

            // 2. AIRCRAFT
            if (Directory.Exists(simObjectsPath))
            {
                foreach (var aircraft in Directory.GetDirectories(simObjectsPath))
                {
                    if (File.Exists(Path.Combine(aircraft, "aircraft.cfg")) &&
                        File.Exists(Path.Combine(aircraft, "model.cfg")))
                    {
                        return AddonType.Aircraft;
                    }
                }
            }

            // 3. AIRPORT SCENERY (VERY STRICT)
            if (Directory.Exists(sceneryPath))
            {
                var files = Directory.GetFiles(sceneryPath, "*.*", SearchOption.AllDirectories);

                foreach (var file in files)
                {
                    var name = Path.GetFileNameWithoutExtension(file);

                    // Airport BGLs almost always include ICAO codes
                    if (IcaoRegex.IsMatch(name))
                        return AddonType.Scenery;
                }
            }

            // 4. LIBRARY / MOD
            if (Directory.Exists(modelLibPath) || Directory.Exists(sceneryPath))
                return AddonType.Library;

            return AddonType.Unknown;
        }
    }
}
