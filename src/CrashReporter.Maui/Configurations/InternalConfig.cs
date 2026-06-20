
namespace CrashReporter.Maui.Configurations;

internal interface IReadOnlyConfig
{
    bool IsCheckOnInitializationEnabled { get; }
    bool IsAndroidUnhandeledExceptionReporterEnabled { get; }
    bool IsAndroidUncaughtExceptionReporterEnabled { get; }
    bool IsMauiTerminatingUnhandeledExceptionReporterEnabled { get; }
    bool IsDeviceInfoSnapshotEnabled { get; }
    bool IsAppInfoSnapshotEnabled { get; }
}
internal sealed class InternalConfig : IReadOnlyConfig
{
    public bool IsCheckOnInitializationEnabled { get; internal set; } = true;
    public bool IsAndroidUnhandeledExceptionReporterEnabled { get; internal set; } = true;
    public bool IsAndroidUncaughtExceptionReporterEnabled { get; internal set; } = true;
    public bool IsMauiTerminatingUnhandeledExceptionReporterEnabled { get; internal set; } = true;
    public bool IsDeviceInfoSnapshotEnabled { get; internal set; } = true;
    public bool IsAppInfoSnapshotEnabled { get; internal set; } = true;
}