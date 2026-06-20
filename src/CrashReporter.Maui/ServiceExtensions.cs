using CrashReporter.Maui.Crashes.Shared;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CrashReporter.Maui;

public static class ServiceExtensions
{

    public static MauiAppBuilder AddCrashHandling(this MauiAppBuilder builder, Action<Configuration>? setup = null)
    {
        builder.Services.AddCrashHandling(setup);
        return builder;
    }

    public static IServiceCollection AddCrashHandling(this IServiceCollection services, Action<Configuration>? setup = null)
    {
        var config = new InternalConfig();
        var s = new Configuration(services, config);
        setup?.Invoke(s);

        services.AddServices(config);
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services, InternalConfig config)
    {
        services.AddSingleton<IReadOnlyConfig>(config);

        services.TryAddSingleton<ISnapshotCollector, SnapshotCollector>();
        services.TryAddSingleton<ICrashManager, CrashManager>();

        if (config.IsDeviceInfoSnapshotEnabled)
            services.AddSingleton<ISnapshotProvider, DeviceInfoSnapshotProvider>();
        if (config.IsAppInfoSnapshotEnabled)
            services.AddSingleton<ISnapshotProvider, AppInfoSnapshotProvider>();
        
        if (config.IsMauiTerminatingUnhandeledExceptionReporterEnabled)
        {
            services.AddSingleton<TerminatingUnhandledExceptionReporter>();
            services.AddSingleton<ICrashReportProvider>(sp => sp.GetRequiredService<TerminatingUnhandledExceptionReporter>());
        }

        services.AddNativeServices(config);
        return services;
    }

    /// <summary>
    /// Initializes the crash handling system by enabling all registered crash report providers and checking for any stored crashes using the registered crash checker.
    /// Should most likely be called after the app has started <see cref="Application.OnStart"/>. This gives your handlers the chance to use the UI to provide the user with information about the crash or options for how to proceed, for example. However, if you want to check for crashes earlier by calling this method earlier in the app's lifecycle. Just keep in mind that if you call it too early, some of your handlers may not be fully initialized yet and may not be able to handle the crash properly.
    /// </summary>
    /// <param name="services"></param>
    public static async Task InitializeCrashHandling(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        var config = services.GetRequiredService<IReadOnlyConfig>();

        if (config.IsCheckOnInitializationEnabled)
        {
            var crashManager = services.GetRequiredService<ICrashManager>();
            var crash = await crashManager.GetReport(cancellationToken);
            if (crash != null)
                await crashManager.HandleCrash(crash, cancellationToken);
        }

        // Initialize the SnapshotCollector and therefore all the snapshot providers.
        _ = services.GetRequiredService<ISnapshotCollector>();

        if (config.IsMauiTerminatingUnhandeledExceptionReporterEnabled)
            await services.GetRequiredService<TerminatingUnhandledExceptionReporter>().Initialize(cancellationToken);

        await services.InitializeNativeServices(config, cancellationToken);
    }




}
