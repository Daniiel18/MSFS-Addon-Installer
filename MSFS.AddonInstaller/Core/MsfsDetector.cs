using MSFS.AddonInstaller.Models;

namespace MSFS.AddonInstaller.Core
{
    public static class MsfsDetector
    {
        private static readonly string[] PossibleCfgPaths =
        {
            // MS Store
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Packages",
                "Microsoft.FlightSimulator_8wekyb3d8bbwe",
                "LocalCache",
                "UserCfg.opt"
            ),

            // Steam
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Microsoft Flight Simulator",
                "UserCfg.opt"
            )
        };

        public static MsfsInstallation Detect()
        {
            foreach (var cfgPath in PossibleCfgPaths)
            {
                if (!File.Exists(cfgPath))
                    continue;

                var installedPath = ParseInstalledPackagesPath(cfgPath);

                if (string.IsNullOrWhiteSpace(installedPath))
                    continue;

                return new MsfsInstallation
                {
                    SimulatorVersion = DetectVersion(installedPath),
                    InstalledPackagesPath = installedPath
                };
            }

            throw new InvalidOperationException(
                "No se pudo detectar una instalación válida de MSFS 2020/2024."
            );
        }

        private static string ParseInstalledPackagesPath(string cfgPath)
        {
            foreach (var line in File.ReadLines(cfgPath))
            {
                if (!line.StartsWith("InstalledPackagesPath"))
                    continue;

                return line
                    .Split('"', StringSplitOptions.RemoveEmptyEntries)
                    .Last();
            }

            return string.Empty;
        }

        private static string DetectVersion(string installedPackagesPath)
        {
            // Heurística simple, extensible luego
            return Directory.Exists(
                Path.Combine(installedPackagesPath, "Official", "Steam"))
                ? "MSFS Steam"
                : "MSFS Microsoft Store";
        }
    }
}
