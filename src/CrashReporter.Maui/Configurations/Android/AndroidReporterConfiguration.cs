
namespace CrashReporter.Maui.Configurations.Android;

public sealed class AndroidReporterConfiguration
{
    private readonly ReporterConfiguration _reporterConfiguration;
    private readonly UnhandledExceptionReporterConfiguration _unhandledConfig;
    private readonly UncaughtExceptionReporterConfiguration _uncaughtConfig;
    internal AndroidReporterConfiguration(ReporterConfiguration reporterConfiguration, InternalConfig config)
    {
        _reporterConfiguration = reporterConfiguration;
        _unhandledConfig = new(config);
        _uncaughtConfig = new(config);
    }

    public ReporterConfiguration EnvironmentUnhandled(Action<UnhandledExceptionReporterConfiguration> setup)
    {
        setup?.Invoke(_unhandledConfig);
        return _reporterConfiguration;
    }
    public ReporterConfiguration ThreadUncaught(Action<UncaughtExceptionReporterConfiguration> setup)
    {
        setup?.Invoke(_uncaughtConfig);
        return _reporterConfiguration;
    }

    public ReporterConfiguration DisableAll(bool shouldDisable = true)
    {
        _unhandledConfig.Disable(shouldDisable);
        _uncaughtConfig.Disable(shouldDisable);
        return _reporterConfiguration;
    }
}