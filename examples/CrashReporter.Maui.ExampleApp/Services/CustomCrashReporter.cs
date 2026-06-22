namespace CrashReporter.Maui.ExampleApp.Services;

internal sealed class CustomCrashReporter : ICrashReportSource
{
    private static ICrash? _lastCrash;
    public Task<ICrash?> Consume(CancellationToken cancellationToken)
    {
        // In a real implementation, you would retrieve the crash report from disk or some other storage location as this is (usually) called on app startup after a crash, rather than while handling the crash.
        var crash = _lastCrash;
        _lastCrash = null;
        return Task.FromResult(crash);
    }

    internal static void ReportCrash(string message, ISnapshotCollector snapshotCollector)
    {
        _lastCrash = new CustomCrash(
            Id: Guid.NewGuid(),
            Type: nameof(CustomCrash),
            Raw: message,
            TimeStampUTC: DateTime.UtcNow,
            ExceptionInfo: null,
            Snapshots: snapshotCollector.GetSnapshots()
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