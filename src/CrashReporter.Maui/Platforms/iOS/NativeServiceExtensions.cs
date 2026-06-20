
using CrashReporter.Maui.Configurations;

namespace CrashReporter.Maui;

internal static class NativeServiceExtensions
{
    internal static IServiceCollection AddNativeServices(this IServiceCollection services, IReadOnlyConfig config)
    {
        return services;
    }
    internal static ValueTask InitializeNativeServices(this IServiceProvider services, IReadOnlyConfig config, CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
}