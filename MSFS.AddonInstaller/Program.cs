using MSFS.AddonInstaller.Core;

Console.Title = "MSFS Addon Installer";

try
{
    var installation = MsfsDetector.Detect();

    Console.WriteLine("MSFS detectado correctamente");
    Console.WriteLine($"Versión: {installation.SimulatorVersion}");
    Console.WriteLine($"Ruta base: {installation.InstalledPackagesPath}");
    Console.WriteLine($"Community: {installation.CommunityPath}");
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ResetColor();
}

Console.WriteLine();
Console.WriteLine("Presiona cualquier tecla para salir...");
Console.ReadKey();
