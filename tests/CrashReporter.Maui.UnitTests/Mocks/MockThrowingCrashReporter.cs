
namespace CrashReporter.Maui.UnitTests.Mocks;

/// <summary>A reporter whose <see cref="GetReport"/> always throws, to exercise the
/// CrashManager's per-reporter exception handling.</summary>
public class MockThrowingCrashReporter : ICrashReportProvider
{
    public int GetReportCount { get; private set; }

    public Task<ICrash?> GetReport(CancellationToken cancellationToken)
    {
        GetReportCount++;
        throw new InvalidOperationException("Reporter failed");
    }
}
