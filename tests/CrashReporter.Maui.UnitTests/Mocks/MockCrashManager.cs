

namespace CrashReporter.Maui.UnitTests.Mocks;

public class MockCrashManager : ICrashManager
{
    public ICrash? Crash { get; set; }

    public int GetCount { get; private set; }
    public ValueTask<ICrash?> GetReport(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public int HandleCount { get; private set; }
    public ValueTask HandleCrash(ICrash report, CancellationToken cancellationToken)
    {
        HandleCount++;
        return ValueTask.CompletedTask;
    }
}