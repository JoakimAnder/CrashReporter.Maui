
namespace CrashReporter.Maui.Configurations.Android;

public sealed class UncaughtExceptionReporterConfiguration
{
    private InternalConfig _config;
    internal UncaughtExceptionReporterConfiguration(InternalConfig config)
    {
        _config = config;
    }

    public UncaughtExceptionReporterConfiguration Disable(bool shouldDisable = true)
    {
        _config.IsAndroidThreadUncaughtExceptionReporterEnabled = !shouldDisable;
        return this;
    }
}