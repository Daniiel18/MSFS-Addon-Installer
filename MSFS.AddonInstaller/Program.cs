using MSFS.AddonInstaller.Core;
using MSFS.AddonInstaller.Models;
using MSFS.AddonInstaller.Utils;

Console.Title = "Drag&Drop Addon Installer v2.0.0";

Logger.Initialize();

var supportedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
{
    ".zip",
    ".rar"
};

// --------------------------------------------------
// VALIDATE INPUT
// --------------------------------------------------
if (args.Length == 0)
{
    ShowInvalidInputMessage();
    return;
}

var invalidInputs = new List<string>();
var validArchivePaths = new List<string>();

foreach (var path in args)
{
    if (!File.Exists(path))
    {
        invalidInputs.Add(Path.GetFileName(path));
        continue;
    }

    if (!supportedExtensions.Contains(Path.GetExtension(path)))
    {
        invalidInputs.Add(Path.GetFileName(path));
        continue;
    }

    validArchivePaths.Add(path);
}

if (validArchivePaths.Count == 0)
{
    ShowInvalidInputMessage();
    return;
}

Logger.Info("Application started.");

var appStopwatch = System.Diagnostics.Stopwatch.StartNew();

// ============================
// RESULT COLLECTIONS
// ============================
var installedResults = new List<InstallResult>();
var updatedResults = new List<InstallResult>();

var skippedSameVersion = new List<InstallResult>();
var skippedNewerInstalled = new List<InstallResult>();

var skippedInvalidArchives = new List<string>(invalidInputs);

try
{
    var installation = MsfsDetector.Detect();
    Logger.Info($"MSFS detected. Community path: {installation.CommunityPath}");

    var addons = AddonScanner.Scan(validArchivePaths.ToArray());
    Logger.Info($"Add-ons detected: {addons.Count}");

    // ----------------------------
    // HEADER
    // ----------------------------
    ConsoleUI.DrawInlineHeader(
        addons.Count > 1
            ? "You initiated the installation process from those archives:"
            : "You initiated the installation process from this archive:",
        ConsoleColor.Cyan
    );

    Console.WriteLine();

    int i = 1;
    foreach (var path in validArchivePaths)
    {
        Console.WriteLine($"{i}. {Path.GetFileName(path)}");
        i++;
    }

    Console.WriteLine();

    // ----------------------------
    // INSTALL PER ADDON
    // ----------------------------
    foreach (var addon in addons)
    {
        var rootStates = AddonConflictResolver.GetInstallStates(
            addon,
            installation.CommunityPath
        );

        foreach (var root in rootStates)
        {
            Logger.Info(
                $"Addon root '{root.RootName}' install state: {root.State}"
            );

            if (root.State == AddonInstallState.AlreadyInstalled)
                Logger.Info($"Existing addon detected: {root.RootName}");
        }

        var results = AddonInstaller.Install(
            addon,
            installation.CommunityPath
        );

        foreach (var result in results)
        {
            switch (result.Status)
            {
                case InstallStatus.Installed:
                    installedResults.Add(result);
                    break;

                case InstallStatus.Updated:
                    updatedResults.Add(result);
                    break;

                case InstallStatus.SkippedSameVersion:
                    skippedSameVersion.Add(result);
                    break;

                case InstallStatus.SkippedNewerInstalled:
                    skippedNewerInstalled.Add(result);
                    break;
            }

            Logger.Info(
                $"{result.Status}: {result.Name} | " +
                $"Size: {result.SizeBytes} bytes | " +
                $"Time: {result.Duration}"
            );
        }
    }
}
catch (Exception ex)
{
    Logger.Error(ex);

    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Fatal error occurred. See log for details.");
    Console.ResetColor();
}

appStopwatch.Stop();

Console.WriteLine();

// ============================
// FINAL SUMMARY
// ============================

// UPDATED
if (updatedResults.Count > 0)
{
    Console.ForegroundColor = ConsoleColor.Green;

    ConsoleUI.DrawInlineHeader(
        $"Add-ons updated: {updatedResults.Count}",
        ConsoleColor.Green
    );

    ConsoleUI.WriteEnumeratedColumns(
        updatedResults.Select(r =>
            $"{r.Name} ({SizeFormatter.Format(r.SizeBytes)})"
        ).ToList()
    );

    Console.ResetColor();
    Console.WriteLine();
}

// INSTALLED (NEW)
if (installedResults.Count > 0)
{
    Console.ForegroundColor = ConsoleColor.Green;

    ConsoleUI.DrawInlineHeader(
        $"Add-ons installed: {installedResults.Count}",
        ConsoleColor.Green
    );

    ConsoleUI.WriteEnumeratedColumns(
        installedResults.Select(r =>
            $"{r.Name} ({SizeFormatter.Format(r.SizeBytes)})"
        ).ToList()
    );

    Console.ResetColor();
    Console.WriteLine();
}

// SKIPPED – NEWER VERSION INSTALLED
if (skippedNewerInstalled.Count > 0)
{
    Console.ForegroundColor = ConsoleColor.Red;

    ConsoleUI.DrawInlineHeader(
        $"Add-ons skipped (newer version already installed): {skippedNewerInstalled.Count}",
        ConsoleColor.Red
    );

    ConsoleUI.WriteEnumeratedColumns(
        skippedNewerInstalled.Select(r => r.Name).ToList()
    );

    Console.ResetColor();
    Console.WriteLine();
}

// SKIPPED – SAME VERSION
if (skippedSameVersion.Count > 0)
{
    Console.ForegroundColor = ConsoleColor.Magenta;

    ConsoleUI.DrawInlineHeader(
        $"Add-ons skipped (same version): {skippedSameVersion.Count}",
        ConsoleColor.Magenta
    );

    ConsoleUI.WriteEnumeratedColumns(
        skippedSameVersion.Select(r => r.Name).ToList()
    );

    Console.ResetColor();
    Console.WriteLine();
}

// INVALID ARCHIVES
if (skippedInvalidArchives.Count > 0)
{
    Console.ForegroundColor = ConsoleColor.Red;

    ConsoleUI.DrawInlineHeader(
        $"Invalid archives skipped: {skippedInvalidArchives.Count}",
        ConsoleColor.Red
    );

    ConsoleUI.WriteEnumeratedColumns(skippedInvalidArchives);

    Console.ResetColor();
    Console.WriteLine();
}

if (installedResults.Count > 0 || updatedResults.Count > 0)
{
    Console.ForegroundColor = ConsoleColor.DarkGray;

    Console.WriteLine(
        $"The installation process took: {FormatElapsed(appStopwatch.Elapsed)}"
    );

    Console.ResetColor();
}

Logger.Info($"Total installation time: {appStopwatch.Elapsed}");

Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();


// ==================================================
// HELPERS
// ==================================================
string FormatElapsed(TimeSpan elapsed)
{
    if (elapsed.TotalHours >= 1)
    {
        return $"{(int)elapsed.TotalHours}h {elapsed.Minutes}m {elapsed.Seconds}s";
    }

    if (elapsed.TotalMinutes >= 1)
    {
        return $"{elapsed.Minutes}m {elapsed.Seconds}s";
    }

    return $"{elapsed.Seconds}s";
}

void ShowInvalidInputMessage()
{
    Console.ForegroundColor = ConsoleColor.Red;

    Console.WriteLine("ERROR: Invalid input.");
    Console.WriteLine();
    Console.WriteLine("This installer only accepts compressed add-on archives:");
    Console.WriteLine("  • .zip");
    Console.WriteLine("  • .rar");
    Console.WriteLine();
    Console.WriteLine("Folders or other file types are not supported.");
    Console.WriteLine("Please drag and drop a valid archive onto the executable.");

    Console.ResetColor();

    Console.WriteLine();
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
