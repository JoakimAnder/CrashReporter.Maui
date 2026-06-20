namespace CrashReporter.Maui.ExampleApp.Services;

internal sealed class CustomCrashReporter(ISnapshotManager snapshotManager) : ICrashReportProvider
{
    private static ICrash? _lastCrash;
    public async Task<ICrash?> GetReport(CancellationToken cancellationToken)
    {
        // In a real implementation, you would retrieve the crash report from disk or some other storage location as this is (usually) called on app startup after a crash, rather than while handling the crash.
        return _lastCrash;
    }

    internal static async Task ReportCrash(string message)
    {
        _lastCrash = new CustomCrash(
            Id: Guid.NewGuid(),
            Type: nameof(CustomCrash),
            Raw: message,
            TimeStampUTC: DateTimeOffset.UtcNow,
            ExceptionInfo: null,
            Snapshots: snapshotManager.GetSnapshots()
        );
    }

    private sealed record CustomCrash(
        Guid Id,
        string Type,
        string Raw,
        DateTime TimeStampUTC,
        ExceptionInfo? ExceptionInfo,
        IReadOnlyCollection<Snapshot> Snapshots
    ) : ICrash;
}