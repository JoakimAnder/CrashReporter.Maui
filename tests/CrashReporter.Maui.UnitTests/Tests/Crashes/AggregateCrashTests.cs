
using CrashReporter.Maui.Crashes;

namespace CrashReporter.Maui.UnitTests.Tests.Crashes;

public class AggregateCrashTests
{
    [Fact]
    public void ExposesProvidedInnerCrashes()
    {
        var inner = new ICrash[] { new MockCrash(), new MockCrash() };

        var sut = new AggregateCrash(inner);

        Assert.Equal(2, sut.InnerCrashes.Count);
        Assert.Equal(inner, sut.InnerCrashes);
    }

    [Fact]
    public void HasAggregateMetadataAndGeneratesId()
    {
        var sut = new AggregateCrash([]);

        Assert.Equal(nameof(AggregateCrash), sut.Type);
        Assert.NotEqual(Guid.Empty, sut.Id);
        Assert.Null(sut.ExceptionInfo);
        Assert.Empty(sut.Snapshots);
        Assert.False(string.IsNullOrWhiteSpace(sut.Raw));
    }
}
