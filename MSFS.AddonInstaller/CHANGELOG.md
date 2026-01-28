# Changelog

## v2.0.0 – Smart Updates & Improved Detection

### Added
- Manifest-based version detection using `manifest.json`.
- Automatic addon updates when a newer version is detected.
- Protection against accidental downgrades.
- Support for archives containing multiple addon roots.
- Clear per-addon installation results (installed, updated, skipped).

### Improved
- Accurate installation summary with real sizes and timings.
- Clear color-coded console output for install results.
- Safer overwrite logic.

### Removed
- `.7z` archive support due to performance and reliability issues.

### Fixed
- Incorrect addon install counts showing `0 B`.
- False positives when addons were already installed.
- Infinite loops on large compressed archives.
- Better detection of existing addon roots.
