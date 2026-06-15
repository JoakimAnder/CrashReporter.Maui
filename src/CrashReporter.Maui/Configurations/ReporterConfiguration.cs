using CrashReporter.Maui.Configurations.Android;
using CrashReporter.Maui.Configurations.Shared;
using CrashReporter.Maui.Configurations.iOS;

namespace CrashReporter.Maui.Configurations;

public sealed class ReporterConfiguration
{
    internal ReporterConfiguration(InternalConfig config)
    {
        IOS = new IOSReporterConfiguration(this, config);
        Android = new AndroidReporterConfiguration(this, config);
        Shared = new SharedReporterConfiguration(this, config);
    }

    public IOSReporterConfiguration IOS { get; }
    public AndroidReporterConfiguration Android { get; }
    public SharedReporterConfiguration Shared { get; }

    public ReporterConfiguration DisableAll(bool shouldDisable = true)
    {
        IOS.DisableAll(shouldDisable);
        Android.DisableAll(shouldDisable);
        Shared.DisableAll(shouldDisable);
        return this;
    }

}