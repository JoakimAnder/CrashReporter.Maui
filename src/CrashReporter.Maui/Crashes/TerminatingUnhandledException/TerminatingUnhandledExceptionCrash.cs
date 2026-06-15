
namespace CrashReporter.Maui.Crashes.Shared;

internal sealed record TerminatingUnhandledExceptionCrash(
    Guid Id,
    string Raw,
    DateTime TimeStampUTC,
    ExceptionInfo? ExceptionInfo,
    IReadOnlyCollection<Snapshot> Snapshots
    ) : ICrash
{
    public string Type => nameof(TerminatingUnhandledExceptionCrash);

    public static TerminatingUnhandledExceptionCrash FromException(Exception ex, IReadOnlyCollection<Snapshot> snapshots)
        => new (Guid.NewGuid(), ex.ToString(), DateTime.UtcNow, ExceptionInfo.FromException(ex), snapshots);
}