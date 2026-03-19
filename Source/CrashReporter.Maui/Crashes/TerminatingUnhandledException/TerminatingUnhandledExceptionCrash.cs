
namespace CrashReporter.Maui.Crashes.Shared;

internal sealed record TerminatingUnhandledExceptionCrash(
    Guid Id,
    string Raw,
    DateTime TimeStampUTC,
    ExceptionInfo? ExceptionInfo,
    DeviceInfoSnapshot? DeviceInfo,
    AppInfoSnapshot? AppInfo
    ) : ICrash
{
    public string Type => nameof(TerminatingUnhandledExceptionCrash);

    public static TerminatingUnhandledExceptionCrash FromException(Exception ex)
        => new (Guid.NewGuid(), ex.ToString(), DateTime.UtcNow, ExceptionInfo.FromException(ex), DeviceInfoSnapshot.FromCurrent(), AppInfoSnapshot.FromCurrent());
}