

namespace CrashReporter.Maui.Configurations.iOS;

public sealed class IOSReporterConfiguration
{
    private readonly ReporterConfiguration _reporterConfiguration;
    private readonly PLCrashReporterConfiguration _PLConfig;
    internal IOSReporterConfiguration(ReporterConfiguration reporterConfiguration, InternalConfig config)
    {
        _reporterConfiguration = reporterConfiguration;
        _PLConfig = new(config);
    }

    public ReporterConfiguration PLCrashReporter(Action<PLCrashReporterConfiguration> setup)
    {
        setup?.Invoke(_PLConfig);
        return _reporterConfiguration;
    }

    public ReporterConfiguration DisableAll(bool shouldDisable = true)
    {
        _PLConfig.Disable(shouldDisable);
        return _reporterConfiguration;
    }
}