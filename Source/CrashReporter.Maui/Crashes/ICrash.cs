
namespace CrashReporter.Maui.Crashes;

public interface ICrash
{
    Guid Id { get; }
    string Type { get; }
    string Raw { get; }
    DateTime TimeStampUTC { get; }
    DeviceInfoSnapshot? DeviceInfo { get; }
    AppInfoSnapshot? AppInfo { get; }
    ExceptionInfo? ExceptionInfo { get; }
}