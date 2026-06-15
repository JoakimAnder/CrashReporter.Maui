using CrashReporter.Maui.Configurations.Android;
using CrashReporter.Maui.Configurations.iOS;
using CrashReporter.Maui.Configurations.Shared;

namespace CrashReporter.Maui.Configurations.Shared;

public sealed class SharedReporterConfiguration
{
    private ReporterConfiguration _reporterConfiguration;
    private TerminatingUnhandledExceptionReporterConfiguration _terminatingUnhandled;
    internal SharedReporterConfiguration(ReporterConfiguration reporterConfiguration, InternalConfig config)
    {
        _reporterConfiguration = reporterConfiguration;
        _terminatingUnhandled = new TerminatingUnhandledExceptionReporterConfiguration(config);
    }

    public ReporterConfiguration DisableAll(bool shouldDisable = true)
    {
        _terminatingUnhandled.Disable(shouldDisable);
        return _reporterConfiguration;
    }

    public ReporterConfiguration TerminatingUnhandled(Action<TerminatingUnhandledExceptionReporterConfiguration> setup)
    {
        setup(_terminatingUnhandled);
        return _reporterConfiguration;
    }
}