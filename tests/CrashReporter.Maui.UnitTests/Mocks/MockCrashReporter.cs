

namespace CrashReporter.Maui.UnitTests.Mocks;

public class MockCrashReporter : ICrashReportSource
{
    public ICrash? Crash { get; set; }

    public int ConsumeCount { get; private set; }
    public Task<ICrash?> Consume(CancellationToken cancellationToken)
    {
        ConsumeCount++;
        return Task.FromResult(Crash);
    }
}