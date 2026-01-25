using MSFS.AddonInstaller.Core;
using MSFS.AddonInstaller.Models;
using System.Diagnostics;
using MSFS.AddonInstaller.Utils;


Console.Title = "MSFS Addon Installer";

Console.WriteLine("MSFS Addon Installer");
Console.WriteLine("--------------------");
Console.WriteLine();

var installResults = new List<InstallResult>();
var totalStopwatch = Stopwatch.StartNew();

try
{
    var installation = MsfsDetector.Detect();
    var addons = AddonScanner.Scan(args);

    int index = 1;
    foreach (var addon in addons)
    {
        Console.WriteLine($"{index}. {addon.Name}");

        var result = AddonInstaller.Install(
            addon,
            installation.CommunityPath
        );

        if (result != null)
        {
            installResults.Add(result);
        }

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

totalStopwatch.Stop();

// ===== FINAL SUMMARY =====
Console.Clear();

Console.WriteLine("MSFS Addon Installer - Summary");
Console.WriteLine("------------------------------");
Console.WriteLine();

Console.WriteLine($"Add-ons installed: {installResults.Count}");
Console.WriteLine();

int summaryIndex = 1;
foreach (var result in installResults)
{
    Console.WriteLine(
    $"   Size: {SizeFormatter.Format(result.SizeBytes)}"
    );
    Console.WriteLine($"   Time: {result.Duration:mm\\:ss}");
    Console.WriteLine();

    summaryIndex++;
}

Console.WriteLine(
    $"Total installation time: {totalStopwatch.Elapsed:mm\\:ss}"
);
Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
