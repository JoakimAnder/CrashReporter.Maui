
namespace CrashReporter.Maui.UnitTests.Mocks;

public class MockSnapshotProvider : ISnapshotProvider
{
    public Snapshot? Snapshot { get; set; }

    public int GetSnapshotCount { get; private set; }
    public Snapshot TakeSnapshot()
    {
        GetSnapshotCount++;
        return Snapshot!;
    }
}