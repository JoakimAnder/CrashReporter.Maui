
namespace CrashReporter.Maui.UnitTests.Tests.ServiceExtensions.AddCrashHandling.AddHandler;

public class AddHandlerTests
{

    private static IServiceCollection GetSut() => new ServiceCollection()
        .AddLogging();
    
    [Fact]
    public void AddCrashHandling_AddCustomHandler_HasTheHandler()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling(config => config
                .AddHandler<MockCrashHandler>())
            .BuildServiceProvider();

        var handlers = services.GetServices<ICrashHandler>();
        var handler = Assert.Single(handlers);
        Assert.IsType<MockCrashHandler>(handler);
    }

    [Fact]
    public void AddCrashHandling_AddCustomHandler_IsTransient()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling(config => config
                .AddHandler<MockCrashHandler>())
            .BuildServiceProvider();

        var reporters1 = services.GetServices<ICrashHandler>();
        var mockReporter1 = reporters1.Single();

        var reporters2 = services.GetServices<ICrashHandler>();
        var mockReporter2 = reporters2.Single();

        Assert.NotSame(mockReporter1, mockReporter2);
    }
}