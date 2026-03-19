
namespace CrashReporter.Maui.Configurations.Android;

public sealed class UnhandledExceptionReporterConfiguration
{
    private InternalConfig _config;
    internal UnhandledExceptionReporterConfiguration(InternalConfig config)
    {
        _config = config;
    }

    public UnhandledExceptionReporterConfiguration Disable(bool shouldDisable = true)
    {
        _config.IsAndroidEnvironmentUnhandledExceptionReporterEnabled = !shouldDisable;
        return this;
    }
}