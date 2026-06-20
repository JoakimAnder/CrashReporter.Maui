
namespace CrashReporter.Maui.UnitTests.Mocks;

/// <summary>A handler whose <see cref="HandleCrash"/> always throws, to verify the CrashManager
/// keeps invoking the remaining handlers.</summary>
public class MockThrowingCrashHandler : ICrashHandler
{
    public int HandleCount { get; private set; }

    public Task HandleCrash(ICrash report, CancellationToken cancellationToken)
    {
        HandleCount++;
        throw new InvalidOperationException("Handler failed");
    }
}
