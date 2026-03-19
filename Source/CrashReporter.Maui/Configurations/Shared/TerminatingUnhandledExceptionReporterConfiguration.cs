
namespace CrashReporter.Maui.Configurations.Shared;

public sealed class TerminatingUnhandledExceptionReporterConfiguration
{
    private InternalConfig _config;
    internal TerminatingUnhandledExceptionReporterConfiguration(InternalConfig config)
    {
        _config = config;
    }

    public TerminatingUnhandledExceptionReporterConfiguration Disable(bool shouldDisable = true)
    {
        _config.IsTerminatingUnhandledExceptionReporterEnabled = !shouldDisable;
        return this;
    }
}