
namespace CrashReporter.Maui.Crashes;

internal sealed record FileCrash(string Type, string Raw, DateTime TimeStampUTC) : ICrash
{
    public Guid Id { get; } = Guid.NewGuid();
    public DeviceInfoSnapshot? DeviceInfo { get; }
    public AppInfoSnapshot? AppInfo { get; }
    public ExceptionInfo? ExceptionInfo { get; }
}