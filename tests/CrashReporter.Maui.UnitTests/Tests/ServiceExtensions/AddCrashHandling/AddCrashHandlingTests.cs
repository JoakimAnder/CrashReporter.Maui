
namespace CrashReporter.Maui.UnitTests.Tests.ServiceExtensions.AddCrashHandling;

public class AddCrashHandlingTests
{
    private static IServiceCollection GetSut() => new ServiceCollection()
        .AddLogging();

    [Fact]
    public void DefaultsTo_HavingSomeDefaultReporters()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling().BuildServiceProvider();

        var reporters = services.GetServices<ICrashReportProvider>();
        Assert.NotEmpty(reporters);
    }

    [Fact]
    public void DefaultsTo_HavingSomeDefaultSnapshotProviders()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling().BuildServiceProvider();

        var snapshotProviders = services.GetServices<ISnapshotProvider>();
        Assert.NotEmpty(snapshotProviders);
    }

    [Fact]
    public void DefaultsTo_HavingNoDefaultHandlers()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling().BuildServiceProvider();

        var crashHandlers = services.GetServices<ICrashHandler>();
        Assert.Empty(crashHandlers);
    }


}