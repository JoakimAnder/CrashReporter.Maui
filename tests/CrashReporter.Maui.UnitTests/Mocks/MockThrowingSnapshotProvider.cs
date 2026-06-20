
namespace CrashReporter.Maui.UnitTests.Mocks;

/// <summary>A provider whose <see cref="TakeSnapshot"/> always throws, to verify the
/// SnapshotCollector skips it and still collects the rest.</summary>
public class MockThrowingSnapshotProvider : ISnapshotProvider
{
    public int GetSnapshotCount { get; private set; }

    public Snapshot TakeSnapshot()
    {
        GetSnapshotCount++;
        throw new InvalidOperationException("Provider failed");
    }
}
