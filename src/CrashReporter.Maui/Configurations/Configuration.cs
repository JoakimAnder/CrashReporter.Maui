using CrashReporter.Maui.Crashes;

namespace CrashReporter.Maui.Configurations;

public sealed class Configuration
{
    public IServiceCollection Services { get; }
    private readonly InternalConfig _config;
    internal Configuration(IServiceCollection services, InternalConfig config)
    {
        Services = services;
        _config = config;
    }
    
    /// <summary>
    /// Adds a custom crash handler to the crash handling system. <see cref="ICrashHandler.HandleCrash(ICrash, CancellationToken)"/> will be called when a crash is detected; can be used to log the crash, send it to a server, or perform any other custom logic.
    /// <see cref="ICrashHandler.HandleCrash(ICrash, CancellationToken)"/> will be called by <see cref="ICrashManager.HandleCrash(ICrash, CancellationToken)"/> when a crash is detected.
    /// </summary>
    public Configuration AddHandler<TCrashHandler>()
        where TCrashHandler : class, ICrashHandler
    {
        Services.AddTransient<ICrashHandler, TCrashHandler>();
        return this;
    }

    /// <summary>
    /// Adds a crash reporter to the crash handling system. A crash reporter is responsible for checking if a crash has occurred and to save it safely and swiftly before the app shuts down and disposes of any trace of the crash.
    /// <see cref="ICrashReportSource.Consume(CancellationToken)"/> will be called by <see cref="ICrashManager.Consume(CancellationToken)"/> to check for any stored crashes.
    /// </summary>
    public Configuration AddReporter<TCrashReporter>()
        where TCrashReporter : class, ICrashReportSource
    {
        Services.AddSingleton<ICrashReportSource, TCrashReporter>();
        return this;
    }

    /// <summary>
    /// Adds a snapshot provider to the crash handling system. A snapshot provider is responsible for providing snapshots that can be included in the crash report. Snapshots are useful for providing additional context about the state of the app and the device when the crash occurred, which can be helpful for diagnosing and fixing the issue that caused the crash.
    /// </summary>
    public Configuration AddSnapshot<TSnapshotProvider>()
        where TSnapshotProvider : class, ISnapshotProvider
    {
        Services.AddSingleton<ISnapshotProvider, TSnapshotProvider>();
        return this;
    }

    /// <summary>
    /// Disables the automatic crash check that runs on initialization. By default, the <see cref="ICrashManager"/> will check for any crashes when <see cref="ServiceExtensions.InitializeCrashHandling(IServiceProvider, CancellationToken)"/> is called. Disabling this can be useful if you want to manually control when the crash check occurs, for example when you want to authorize the user first.
    /// </summary>
    public Configuration DisableCrashCheckOnInitialization(bool shouldDisable = true)
    {
        _config.IsCheckOnInitializationEnabled = !shouldDisable;
        return this;
    }

    /// <summary>
    /// Disables the Maui terminating unhandled exception reporter. By default, the <see cref="Crashes.Shared.TerminatingUnhandledExceptionReporter"/> is enabled. Disabling this can be useful if you want to handle terminating unhandled exceptions in a different way.
    /// </summary>
    public Configuration DisableMauiTerminatingUnhandeledExceptionReporter(bool shouldDisable = true)
    {
        _config.IsMauiTerminatingUnhandeledExceptionReporterEnabled = !shouldDisable;
        return this;
    }

    /// <summary>
    /// Disables the Android uncaught exception reporter. By default, the <see cref="Crashes.Android.UncaughtExceptionReporter"/> is enabled. Disabling this can be useful if you want to handle uncaught exceptions in a different way.
    /// </summary>
    public Configuration DisableAndroidUncaughtExceptionReporter(bool shouldDisable = true)
    {
        _config.IsAndroidUncaughtExceptionReporterEnabled = !shouldDisable;
        return this;
    }

    /// <summary>
    /// Disables the Android unhandled exception reporter. By default, the <see cref="Crashes.Android.UnhandledExceptionReporter"/> is enabled. Disabling this can be useful if you want to handle unhandled exceptions in a different way.
    /// </summary>
    public Configuration DisableAndroidUnhandeledExceptionReporter(bool shouldDisable = true)
    {
        _config.IsAndroidUnhandeledExceptionReporterEnabled = !shouldDisable;
        return this;
    }

    /// <summary>
    /// Disables the device info snapshot provider. By default, the <see cref="DeviceInfoSnapshotProvider"/> is enabled. Disabling this can be useful if you don't need the information provided by this snapshot or if you want to implement your own custom device info snapshot provider.
    /// </summary>
    public Configuration DisableDeviceInfoSnapshot(bool shouldDisable = true)
    {
        _config.IsDeviceInfoSnapshotEnabled = !shouldDisable;
        return this;
    }

    /// <summary>
    /// Disables the app info snapshot provider. By default, the <see cref="AppInfoSnapshotProvider"/> is enabled. Disabling this can be useful if you don't need the information provided by this snapshot or if you want to implement your own custom app info snapshot provider.
    /// </summary>
    public Configuration DisableAppInfoSnapshot(bool shouldDisable = true)
    {
        _config.IsAppInfoSnapshotEnabled = !shouldDisable;
        return this;
    }
    
}