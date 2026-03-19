
using CrashReporter.Maui;
using CrashReporter.Maui.Crashes;

namespace CrashReporter.Maui.UnitTests.Tests.ServiceExtensions;

internal class MockCrashHandler : ICrashHandler
{
    public int HandleCount { get; private set; } = 0;
    public Task HandleCrash(ICrash report, CancellationToken cancellationToken)
    {
        HandleCount++;
        return Task.CompletedTask;
    }
}