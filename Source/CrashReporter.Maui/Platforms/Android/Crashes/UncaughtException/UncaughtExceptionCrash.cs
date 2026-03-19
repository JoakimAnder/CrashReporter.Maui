
namespace CrashReporter.Maui.Crashes.Android;

internal sealed record UncaughtExceptionCrash(
    Guid Id,
    string Raw,
    DateTime TimeStampUTC,
    ExceptionInfo? ExceptionInfo,
    DeviceInfoSnapshot? DeviceInfo,
    AppInfoSnapshot? AppInfo
    ) : ICrash
{
    public string Type => nameof(UncaughtExceptionCrash);

    public static UncaughtExceptionCrash FromThrowable(Throwable throwable)
        => new(Guid.NewGuid(), throwable.ToString(), DateTime.UtcNow, MapExceptionInfo(throwable), DeviceInfoSnapshot.FromCurrent(), AppInfoSnapshot.FromCurrent());

    private static ExceptionInfo MapExceptionInfo(Throwable throwable) => new(
        Type: t.Class?.Name ?? "Unknown",
        Message: t.Message ?? string.Empty,
        StackTrace: t.GetStackTrace() is { } frames
            ? string.Join(Environment.NewLine, frames.Select(f => $"  at {f}"))
            : null,
        InnerException: t.Cause is { } cause
            ? MapExceptionInfo(cause)
            : null,
        InnerExceptions: t.Suppressed is { Length: > 0 } suppressed
            ? suppressed.Select(MapExceptionInfo).ToList()
            : null);
}