
namespace CrashReporter.Maui.Crashes.Android;

internal sealed record UnhandledExceptionCrash(
    Guid Id,
    string Raw,
    DateTime TimeStampUTC,
    ExceptionInfo ExceptionInfo,
    DeviceInfoSnapshot DeviceInfo,
    AppInfoSnapshot AppInfo
    ) : ICrash
{
    public static UnhandledExceptionCrash FromText(string raw) => new(Guid.NewGuid(), raw);
}