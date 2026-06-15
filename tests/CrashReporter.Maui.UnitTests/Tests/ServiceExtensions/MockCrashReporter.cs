
using CrashReporter.Maui.Crashes;
using CrashReporter.Maui;

namespace CrashReporter.Maui.UnitTests.Tests.ServiceExtensions;

internal class MockCrashReporter : ICrashReporter
{
    public ICrash? Crash { get; set; }

    public int GetReportCount { get; private set; } = 0;
    public Task<ICrash?> GetReport(CancellationToken cancellationToken)
    {
        GetReportCount++;
        return Task.FromResult(Crash);
    }

    public int InitCount { get; private set; } = 0;
    public Task Initialize(CancellationToken cancellationToken)
    {
        InitCount++;
        return Task.CompletedTask;
    }
}