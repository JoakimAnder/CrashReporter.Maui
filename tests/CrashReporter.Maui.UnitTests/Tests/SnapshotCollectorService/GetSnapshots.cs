
using Microsoft.Extensions.Logging.Abstractions;

namespace CrashReporter.Maui.UnitTests.Tests.SnapshotCollectorService.GetSnapshots;

public class GetSnapshotsTests
{
    private static SnapshotCollector GetSut(IEnumerable<ISnapshotProvider> providers)
        => new(providers, NullLogger<SnapshotCollector>.Instance);

    private static Snapshot SampleSnapshot(string name)
        => new(name, new Dictionary<string, string>());

    [Fact]
    public void NoProvidersReturnsEmpty()
    {
        var sut = GetSut([]);

        Assert.Empty(sut.GetSnapshots());
    }

    [Fact]
    public void CollectsFromAllProviders()
    {
        var a = new MockSnapshotProvider { Snapshot = SampleSnapshot("a") };
        var b = new MockSnapshotProvider { Snapshot = SampleSnapshot("b") };
        var sut = GetSut([a, b]);

        var snapshots = sut.GetSnapshots();

        Assert.Equal(2, snapshots.Count);
        Assert.Contains(snapshots, s => s.Name == "a");
        Assert.Contains(snapshots, s => s.Name == "b");
        Assert.Equal(1, a.GetSnapshotCount);
        Assert.Equal(1, b.GetSnapshotCount);
    }

    [Fact]
    public void SkipsProviderThatThrows()
    {
        var throwing = new MockThrowingSnapshotProvider();
        var ok = new MockSnapshotProvider { Snapshot = SampleSnapshot("ok") };
        var sut = GetSut([throwing, ok]);

        var snapshots = sut.GetSnapshots();

        var only = Assert.Single(snapshots);
        Assert.Equal("ok", only.Name);
        Assert.Equal(1, throwing.GetSnapshotCount);
    }
}
