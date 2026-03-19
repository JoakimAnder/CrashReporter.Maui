
namespace CrashReporter.Maui.Configurations;

internal interface IReadOnlyConfig
{
    bool IsCheckOnInitializationEnabled { get; }
    bool IsTerminatingUnhandledExceptionReporterEnabled { get; }
    bool IsPLCrashReporterEnabled { get; }
    bool IsAndroidEnvironmentUnhandledExceptionReporterEnabled { get; }
    bool IsAndroidThreadUncaughtExceptionReporterEnabled { get; }
}
internal sealed class InternalConfig : IReadOnlyConfig
{
    public bool IsCheckOnInitializationEnabled { get; set; } = true;
    public bool IsTerminatingUnhandledExceptionReporterEnabled { get; set; } = true;
    public bool IsPLCrashReporterEnabled { get; set; } = true;
    public bool IsAndroidEnvironmentUnhandledExceptionReporterEnabled { get; set; } = true;
    public bool IsAndroidThreadUncaughtExceptionReporterEnabled { get; set; } = true;
}