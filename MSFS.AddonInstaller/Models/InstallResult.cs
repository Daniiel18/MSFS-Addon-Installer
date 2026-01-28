public enum InstallStatus
{
    Installed,
    Updated,
    SkippedSameVersion,
    SkippedNewerInstalled
}

public sealed class InstallResult
{
    public string Name { get; init; } = string.Empty;
    public InstallStatus Status { get; init; }

    public long SizeBytes { get; init; }
    public TimeSpan Duration { get; init; }
}
