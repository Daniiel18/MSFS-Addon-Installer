using MSFS.AddonInstaller.Core;
using MSFS.AddonInstaller.Models;
using MSFS.AddonInstaller.Utils;

Console.Title = "MSFS Addon Installer";

Console.WriteLine("MSFS Addon Installer");
Console.WriteLine("--------------------");
Console.WriteLine();

Logger.Initialize(); // Ensure logger is ready
Logger.Info("Application started.");

try
{
    var installation = MsfsDetector.Detect();
    Logger.Info($"MSFS detected. Community path: {installation.CommunityPath}");

    var addons = AddonScanner.Scan(args);
    Logger.Info($"Add-ons detected: {addons.Count}");

    var results = new List<InstallResult>();

    int index = 1;

    foreach (var addon in addons)
    {
        Console.WriteLine($"{index}. {addon.Name}");
        Logger.Info($"Installing addon: {addon.Name}");

        try
        {
            var result = AddonInstaller.Install(
                addon,
                installation.CommunityPath
            );

            if (result != null)
            {
                results.Add(result);
                Logger.Info(
                    $"Installed: {result.Name} | Size: {result.SizeBytes} bytes | Time: {result.Duration}"
                );
            }
            else
            {
                Logger.Info($"Skipped addon: {addon.Name}");
                Console.WriteLine("   Installation skipped.");
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("   Installation failed. See log for details.");
            Console.ResetColor();
        }

        Console.WriteLine();
        index++;
    }

    Logger.Info($"Installation loop finished. Installed count: {results.Count}");

    // FINAL SUMMARY
    Console.Clear();

    Console.WriteLine("MSFS Addon Installer - Summary");
    Console.WriteLine("------------------------------");
    Console.WriteLine();

    Console.WriteLine($"Add-ons installed: {results.Count}");
    Console.WriteLine();

    TimeSpan totalTime = TimeSpan.Zero;

    int i = 1;
    foreach (var result in results)
    {
        totalTime += result.Duration;

        Console.WriteLine($"{i}. {result.Name}");
        Console.WriteLine($"   Size: {SizeFormatter.Format(result.SizeBytes)}");
        Console.WriteLine($"   Time: {result.Duration:mm\\:ss}");
        Console.WriteLine();

        i++;
    }

    Console.WriteLine($"Total installation time: {totalTime:mm\\:ss}");

    Logger.Info($"Total installation time: {totalTime}");
}
catch (Exception ex)
{
    Logger.Error(ex);

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Fatal error occurred. See log for details.");
    Console.ResetColor();
}

Console.WriteLine();
Console.WriteLine("Process completed. Press any key to exit...");
Console.ReadKey();
