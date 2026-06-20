
using Microsoft.Extensions.Logging;

namespace CrashReporter.Maui.UnitTests.Tests.CrashManagerService.HandleCrash;

public class HandleCrashTests
{
    private static CrashManager GetSut(Action<IServiceCollection>? configureServices = null)
    {
        var services = new ServiceCollection();
        configureServices?.Invoke(services);

        var sp = services.AddLogging().BuildServiceProvider();
        return new CrashManager(sp, sp.GetRequiredService<ILogger<CrashManager>>());
    }

    [Fact]
    public async Task InvokesAllHandlersWithTheReport()
    {
        var crash = new MockCrash();
        var handlerA = new MockCrashHandler();
        var handlerB = new MockCrashHandler();
        var sut = GetSut(services => services
            .AddSingleton<ICrashHandler>(handlerA)
            .AddSingleton<ICrashHandler>(handlerB));

        await sut.HandleCrash(crash, CancellationToken.None);

        Assert.Same(crash, Assert.Single(handlerA.HandledReports));
        Assert.Same(crash, Assert.Single(handlerB.HandledReports));
    }

    [Fact]
    public async Task ContinuesWhenHandlerThrows()
    {
        var crash = new MockCrash();
        var throwing = new MockThrowingCrashHandler();
        var handler = new MockCrashHandler();
        var sut = GetSut(services => services
            .AddSingleton<ICrashHandler>(throwing)
            .AddSingleton<ICrashHandler>(handler));

        await sut.HandleCrash(crash, CancellationToken.None);

        Assert.Equal(1, throwing.HandleCount);
        Assert.Equal(1, handler.HandleCount); // ran despite the earlier throw
    }

    [Fact]
    public async Task NullReportInvokesNoHandler()
    {
        var handler = new MockCrashHandler();
        var sut = GetSut(services => services
            .AddSingleton<ICrashHandler>(handler));

        await sut.HandleCrash(null!, CancellationToken.None);

        Assert.Equal(0, handler.HandleCount);
    }
}
