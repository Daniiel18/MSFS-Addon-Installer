using MSFS.AddonInstaller.Core;

Console.Title = "MSFS Addon Installer";

try
{
    var installation = MsfsDetector.Detect();
    var addons = AddonScanner.Scan(args);

    Console.WriteLine("MSFS detectado:");
    Console.WriteLine($"Community: {installation.CommunityPath}");
    Console.WriteLine();

    Console.WriteLine("Addons detectados:");
    int index = 1;
    foreach (var addon in addons)
    {
        Console.WriteLine($"{index++}. {addon.Name}");
    }
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
