using System.Runtime.CompilerServices;
using CrashReporter.Maui.Crashes;
using CrashReporter.Maui.Configurations;

namespace CrashReporter.Maui;

public static class ServiceExtensions
{
    private static readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

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

    private static IServiceCollection AddServices(this IServiceCollection services, IReadOnlyConfig config)
    {
        services.AddSingleton(config);

        services.AddSingleton<ISnapshotCollector, SnapshotCollector>();
        services.AddSingleton<ISnapshotProvider, DeviceInfoSnapshotProvider>();
        services.AddSingleton<ISnapshotProvider, AppInfoSnapshotProvider>();

        if (config.IsTerminatingUnhandledExceptionReporterEnabled)
            services.AddSingleton<ICrashReporter, Crashes.Shared.TerminatingUnhandledExceptionReporter>();

        services.AddNativeServices(config);
        return services;
    }

    /// <summary>
    /// Initializes the crash handling system by enabling all registered crash holders and checking for any stored crashes using the registered crash checker.
    /// Should most likely be called after the app has started <see cref="Application.OnStart"/>. This gives your handlers the chance to use the UI to provide the user with information about the crash or options for how to proceed, for example. However, if you want to check for crashes earlier by calling this method earlier in the app's lifecycle. Just keep in mind that if you call it too early, some of your handlers may not be fully initialized yet and may not be able to handle the crash properly.
    /// </summary>
    /// <param name="services"></param>
    public static async Task InitializeCrashHandling(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        var config = services.GetRequiredService<IReadOnlyConfig>();

        if (config.IsCheckOnInitializationEnabled)
            await services.CheckForCrash(cancellationToken);

        // Warm the snapshot cache before reporters wire up their crash handlers,
        // so a crash firing immediately after init still has snapshots to attach.
        await services.GetRequiredService<ISnapshotCollector>().RefreshAsync(cancellationToken);

        await services.InitializeReporters(cancellationToken);
    }

    public static async Task<ICrash?> CheckForCrash(this IServiceProvider services, CancellationToken cancellationToken)
    {
        if (!await _semaphoreSlim.WaitAsync(millisecondsTimeout:0, cancellationToken))
            return null;
        
        var reporters = services.GetReporters();
        var crashes = await services.GetCrashes(reporters, cancellationToken).ToArrayAsync(cancellationToken);
        
        if (crashes.Length == 0)
            return null;

        if (crashes.Length == 1)
            return crashes[0];

        return new AggregateCrash(crashes);
    }

    private static IEnumerable<ICrashReporter> GetReporters(this IServiceProvider services)
    {
        try
        {
            return services.GetServices<ICrashReporter>();
        }
        catch(Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<CrashReporter>>();
            logger.LogError(ex, "Failed to get crash reporters");
            return [];
        }
    }

    private static async IAsyncEnumerable<ICrash> GetCrashes(this IServiceProvider services, IEnumerable<ICrashReporter> reporters, [EnumeratorCancellation]CancellationToken cancellationToken)
    {
        foreach (var reporter in reporters)
        {
            ICrash? report = null;
            try
            {
                report = await reporter.GetReport(cancellationToken);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<CrashReporter>>();
                logger.LogError(ex, "Failed to get report from {CrashReporter}", reporter.GetType().Name);
            }

            if (report is null)
                continue;

            yield return report;
        }
    }

    private static async Task InitializeReporters(this IServiceProvider services, CancellationToken cancellationToken)
    {
        var reporters = services.GetReporters();
        await services.Initialize(reporters, cancellationToken).ToArrayAsync(cancellationToken);
    }

    private static async IAsyncEnumerable<bool> Initialize(this IServiceProvider services, IEnumerable<ICrashReporter> reporters, [EnumeratorCancellation]CancellationToken cancellationToken)
    {
        foreach (var reporter in reporters)
        {
            bool isSuccess = false;
            try
            {
                await reporter.Initialize(cancellationToken);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<CrashReporter>>();
                logger.LogError(ex, "Failed to initialize reporter");
            }
            yield return isSuccess;
        }
    }

}
