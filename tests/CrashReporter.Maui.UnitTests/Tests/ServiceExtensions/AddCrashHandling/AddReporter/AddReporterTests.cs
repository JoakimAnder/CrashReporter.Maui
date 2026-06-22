
namespace CrashReporter.Maui.UnitTests.Tests.ServiceExtensions.AddCrashHandling.AddReporter;

public class AddReporterTests
{

    private static IServiceCollection GetSut() => new ServiceCollection()
        .AddLogging();
    
    [Fact]
    public void AddCrashHandling_AddCustomReporter_HasTheReporter()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling(config => config
                .DisableMauiTerminatingUnhandeledExceptionReporter()
                .AddReporter<MockCrashReporter>())
            .BuildServiceProvider();

        var reporters = services.GetServices<ICrashReportSource>();
        var reporter = Assert.Single(reporters);
        Assert.IsType<MockCrashReporter>(reporter);
    }

    [Fact]
    public void AddCrashHandling_AddCustomReporter_IsSingleton()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling(config => config
                .DisableMauiTerminatingUnhandeledExceptionReporter()
                .AddReporter<MockCrashReporter>())
            .BuildServiceProvider();

        using var scope1 = services.CreateScope();
        var reporters1 = scope1.ServiceProvider.GetServices<ICrashReportSource>();
        var mockReporter1 = reporters1.Single();

        using var scope2 = services.CreateScope();
        var reporters2 = scope2.ServiceProvider.GetServices<ICrashReportSource>();
        var mockReporter2 = reporters2.Single();

        Assert.Same(mockReporter1, mockReporter2);
    }
}