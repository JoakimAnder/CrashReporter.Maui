
namespace CrashReporter.Maui.UnitTests.Mocks;

/// <summary>A reporter whose <see cref="Consume"/> always throws, to exercise the
/// CrashManager's per-reporter exception handling.</summary>
public class MockThrowingCrashReporter : ICrashReportSource
{
    public int ConsumeCount { get; private set; }

    public Task<ICrash?> Consume(CancellationToken cancellationToken)
    {
        ConsumeCount++;
        throw new InvalidOperationException("Reporter failed");
    }
}
