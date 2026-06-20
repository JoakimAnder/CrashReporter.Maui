
using CrashReporter.Maui.Crashes;

namespace CrashReporter.Maui.UnitTests.Tests.Crashes;

public class FileCrashTests
{
    [Fact]
    public void CarriesProvidedValuesAndGeneratesId()
    {
        var timestamp = new DateTime(2026, 6, 20, 12, 0, 0, DateTimeKind.Utc);

        var sut = new FileCrash("MyType", "raw text", timestamp);

        Assert.Equal("MyType", sut.Type);
        Assert.Equal("raw text", sut.Raw);
        Assert.Equal(timestamp, sut.TimeStampUTC);
        Assert.NotEqual(Guid.Empty, sut.Id);
        Assert.Null(sut.ExceptionInfo);
        Assert.Empty(sut.Snapshots);
    }
}
