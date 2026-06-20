namespace CrashReporter.Maui.Crashes.Android;

internal sealed record UncaughtExceptionCrash(
    Guid Id,
    string Raw,
    DateTime TimeStampUTC,
    ExceptionInfo? ExceptionInfo,
    IReadOnlyCollection<Snapshot> Snapshots
    ) : ICrash
{
    public string Type => nameof(UncaughtExceptionCrash);

    public static UncaughtExceptionCrash FromThrowable(Java.Lang.Throwable throwable, IReadOnlyCollection<Snapshot> snapshots)
        => new(Guid.NewGuid(), throwable.ToString(), DateTime.UtcNow, MapExceptionInfo(throwable), snapshots);

    private static ExceptionInfo MapExceptionInfo(Java.Lang.Throwable throwable) => new(
        Type: throwable.Class?.Name ?? "Unknown",
        Message: throwable.Message ?? string.Empty,
        StackTrace: throwable.GetStackTrace() is { } frames
            ? string.Join(Environment.NewLine, frames.Select(f => $"  at {f}"))
            : null,
        InnerException: throwable.Cause is { } cause
            ? MapExceptionInfo(cause)
            : null,
        InnerExceptions: throwable.Suppressed is { Length: > 0 } suppressed
            ? suppressed.Select(MapExceptionInfo).ToList()
            : null);
}