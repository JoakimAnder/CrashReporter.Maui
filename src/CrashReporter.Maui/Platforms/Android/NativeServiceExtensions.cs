using CrashReporter.Maui.Configurations;
using CrashReporter.Maui.Crashes.Android;

namespace CrashReporter.Maui;

internal static class NativeServiceExtensions
{
    internal static IServiceCollection AddNativeServices(this IServiceCollection services, IReadOnlyConfig config)
    {
        if (config.IsAndroidUnhandeledExceptionReporterEnabled)
        {
            services.AddSingleton<UnhandledExceptionReporter>();
            services.AddSingleton<ICrashReportSource>(sp => sp.GetRequiredService<UnhandledExceptionReporter>());
        }
        if (config.IsAndroidUncaughtExceptionReporterEnabled)
        {
            services.AddSingleton<UncaughtExceptionReporter>();
            services.AddSingleton<ICrashReportSource>(sp => sp.GetRequiredService<UncaughtExceptionReporter>());
        }

        return services;
    }

    internal static async ValueTask InitializeNativeServices(this IServiceProvider services, IReadOnlyConfig config, CancellationToken cancellationToken)
    {
        if (config.IsAndroidUnhandeledExceptionReporterEnabled)
            await services.GetRequiredService<UnhandledExceptionReporter>().Initialize(cancellationToken);
        if (config.IsAndroidUncaughtExceptionReporterEnabled)
            await services.GetRequiredService<UncaughtExceptionReporter>().Initialize(cancellationToken);
    }
}