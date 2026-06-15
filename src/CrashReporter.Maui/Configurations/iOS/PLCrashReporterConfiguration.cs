
namespace CrashReporter.Maui.Configurations.iOS;

public sealed class PLCrashReporterConfiguration
{
    private InternalConfig _config;
    internal PLCrashReporterConfiguration(InternalConfig config)
    {
        _config = config;
    }

    public PLCrashReporterConfiguration Disable(bool shouldDisable = true)
    {
        _config.IsPLCrashReporterEnabled = !shouldDisable;
        return this;
    }
}