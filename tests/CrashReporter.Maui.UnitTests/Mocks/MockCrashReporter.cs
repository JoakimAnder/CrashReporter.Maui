

namespace CrashReporter.Maui.UnitTests.Mocks;

public class MockCrashReporter : ICrashReportProvider
{
    public ICrash? Crash { get; set; }

    public int GetReportCount { get; private set; }
    public Task<ICrash?> GetReport(CancellationToken cancellationToken)
    {
        GetReportCount++;
        return Task.FromResult(Crash);
    }
}