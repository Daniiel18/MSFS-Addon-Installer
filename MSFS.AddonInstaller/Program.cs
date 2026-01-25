using MSFS.AddonInstaller.Core;

Console.Title = "MSFS Addon Installer";

Console.WriteLine("MSFS Addon Installer");
Console.WriteLine("--------------------");
Console.WriteLine();

try
{
    var installation = MsfsDetector.Detect();
    var addons = AddonScanner.Scan(args);

    int index = 1;
    foreach (var addon in addons)
    {
        Console.WriteLine($"{index}. {addon.Name}");
        AddonInstaller.Install(addon, installation.CommunityPath);
        Console.WriteLine();
        index++;
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ResetColor();
}

Console.WriteLine("Process completed. Press any key to exit...");
Console.ReadKey();
