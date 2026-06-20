
namespace CrashReporter.Maui.ExampleApp.Services;
internal sealed class CustomCrashHandler(ILogger<CustomCrashHandler> logger) : ICrashHandler
{
    public Task HandleCrash(ICrash crash, CancellationToken cancellationToken)
    {
        // In a real implementation, you would do something with the crash report here, like log it, alert the user, and/or send it to a server.
        // For this example, we'll just write it to the console.
        logger.LogCritical("Custom Crash Handler: Crash detected! Crash ID: {CrashId}, Timestamp: {Timestamp}", crash.Id, crash.Timestamp);
        return Task.CompletedTask;
    }
}