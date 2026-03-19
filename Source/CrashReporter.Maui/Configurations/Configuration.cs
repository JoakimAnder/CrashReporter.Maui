using CrashReporter.Maui.Crashes;

namespace CrashReporter.Maui.Configurations;

public sealed class Configuration
{
    private readonly IServiceCollection _services;
    private readonly ReporterConfiguration _reporterConfig;
    private readonly InternalConfig _config;
    internal Configuration(IServiceCollection services, InternalConfig config)
    {
        _services = services;
        _config = config;
        _reporterConfig = new(config);
    }
    
    /// <summary>
    /// Adds a custom crash handler to the crash handling system. <see cref="ICrashHandler.HandleCrash(ICrash, CancellationToken)"/> will be called when a crash is detected; can be used to log the crash, send it to a server, or perform any other custom logic.
    /// </summary>
    /// <typeparam name="TCrashHandler"></typeparam>
    /// <returns></returns>
    public Configuration AddHandler<TCrashHandler>()
        where TCrashHandler : class, ICrashHandler
    {
        _services.AddTransient<ICrashHandler, TCrashHandler>();
        return this;
    }

    /// <summary>
    /// Adds a crash reporter to the crash handling system. A crash reporter is responsible for checking if a crash has occurred and to save it safely and swiftly before the app shuts down and disposes of any trace of the crash.
    /// <see cref="ICrashReporter.Initialize(CancellationToken)"/> will be called on initialization to enable the crash holder; can be used to integrate with platform-specific crash reporting mechanisms, such as Android's UncaughtExceptionHandler or iOS's NSSetUncaughtExceptionHandler."/>
    /// <see cref="ICrashReporter.GetReport(CancellationToken)"/> will be called by <see cref="ICrashChecker.CheckForCrashes(CancellationToken)"/> to check for any stored crashes.
    /// </summary>
    /// <typeparam name="TCrashReporter"></typeparam>
    /// <returns></returns>
    public Configuration AddReporter<TCrashReporter>()
        where TCrashReporter : class, ICrashReporter
    {
        _services.AddSingleton<ICrashReporter, TCrashReporter>();
        return this;
    }

    /// <summary>
    /// Disables the automatic crash check that runs on initialization. By default, the <see cref="ICrashChecker.CheckForCrashes(CancellationToken)"/> will check for any crashes when <see cref="ServiceExtensions.InitializeCrashHandling(IServiceProvider)"/> is called. Disabling this can be useful if you want to manually control when the crash check occurs, for example when you want to authorize the user firs.
    /// </summary>
    /// <param name="shouldDisable"></param>
    /// <returns></returns>
    public Configuration DisableCrashCheckOnInitialization(bool shouldDisable = true)
    {
        _config.IsCheckOnInitializationEnabled = !shouldDisable;
        return this;
    }

    public Configuration ConfigureReporters(Action<ReporterConfiguration> setup)
    {
        setup?.Invoke(_reporterConfig);
        return this;
    }
    
}