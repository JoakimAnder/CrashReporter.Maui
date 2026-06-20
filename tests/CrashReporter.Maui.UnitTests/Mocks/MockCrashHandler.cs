
using CrashReporter.Maui;
using CrashReporter.Maui.Crashes;

namespace CrashReporter.Maui.UnitTests.Mocks;

public class MockCrashHandler : ICrashHandler
{
    /// <summary>Every report passed to <see cref="HandleCrash"/>, in order.</summary>
    public List<ICrash> HandledReports { get; } = [];

    public int HandleCount { get; private set; }
    public Task HandleCrash(ICrash report, CancellationToken cancellationToken)
    {
        HandleCount++;
        HandledReports.Add(report);
        return Task.CompletedTask;
    }
}