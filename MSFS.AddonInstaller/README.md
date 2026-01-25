# MSFS Addon Installer

A lightweight, console-based installer for Microsoft Flight Simulator (MSFS 2020 / 2024) that allows users to install multiple addons via drag & drop with automatic simulator detection, real progress tracking, and safe installation behavior.

## Features

* **Drag & Drop Support**: Drop one or more files/folders onto the executable.
* **Automatic MSFS Detection**: Works with Steam and Microsoft Store installations.
* **Supported Formats**:

  * Folders
  * ZIP / 7z / RAR archives
* **Addon Type Detection**:

  * Aircraft
  * Livery
  * Airport Scenery
  * Library / Mod
* **Real Progress Bar & ETA**: Byte-based progress with accurate ETA.
* **Safe by Default**:

  * Skips addons that are already installed.
  * No overwriting unless explicitly added in future versions.
* **Clean Console UI**: No technical noise or internal paths shown.

## Requirements

* Windows 10 / 11
* No .NET installation required (self-contained executable)

## How to Use

1. Download the latest release from GitHub.
2. Drag one or more addon files/folders onto `MSFS.AddonInstaller.exe`.
3. The installer will:

   * Detect your MSFS installation automatically
   * Identify addon types
   * Install each addon sequentially
   * Show progress and ETA

## Example Output

```
1. FBW_A320neo_N377NW_HYDFM
   Livery
   ######################### 100% ETA: 00:00
   Installation completed.

2. jyuraposa-timezonecorrection_kxsAZ
   Library
   Already installed – skipped.

Process completed. Press any key to exit...
```

## Supported MSFS Versions

* Microsoft Flight Simulator 2020
* Microsoft Flight Simulator 2024
* Steam & Microsoft Store editions

## Build & Publish (for developers)

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

The executable will be generated in:

```
bin/Release/net8.0/win-x64/publish/
```

## Versioning

This project follows **Semantic Versioning**:

* `MAJOR.MINOR.PATCH`
* Current release: **v1.0.0**

## Roadmap

* Optional overwrite flag (`--force`)
* Duplicate detection by manifest hash
* Dry-run mode
* Optional GUI frontend

## Disclaimer

This tool is not affiliated with Microsoft or Asobo Studio.
Use at your own risk.

## License

MIT License
