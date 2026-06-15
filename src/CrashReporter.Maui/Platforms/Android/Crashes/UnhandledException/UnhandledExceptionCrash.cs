
namespace CrashReporter.Maui.Crashes.Android;

internal sealed record UnhandledExceptionCrash(
    Guid Id,
    string Raw,
    DateTime TimeStampUTC,
    ExceptionInfo? ExceptionInfo,
    IReadOnlyCollection<Snapshot> Snapshots
    ) : ICrash
{
    public string Type => nameof(UnhandledExceptionCrash);

    public static UnhandledExceptionCrash FromException(Exception ex, IReadOnlyCollection<Snapshot> snapshots)
        => new(Guid.NewGuid(), ex.ToString(), DateTime.UtcNow, ExceptionInfo.FromException(ex), snapshots);
}