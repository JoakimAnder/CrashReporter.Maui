
using CrashReporter.Maui.Crashes;
using Microsoft.Extensions.Logging;

namespace CrashReporter.Maui.UnitTests.Tests.CrashManagerService.GetReport;

public class GetReportTests
{
    private readonly MockCrashReporter _mockReporter = new();
    private readonly MockCrashHandler _mockHandler = new();

    private static CrashManager GetSut(Action<IServiceCollection>? configureServices = null)
    {
        var services = new ServiceCollection();
        configureServices?.Invoke(services);

        var sp = services.AddLogging().BuildServiceProvider();
        return new CrashManager(sp, sp.GetRequiredService<ILogger<CrashManager>>());
    }

    [Fact]
    public async Task ReturnsNullWhenNoReport()
    {
        var sut = GetSut(services => services
            .AddSingleton<ICrashReportProvider>(_mockReporter)
            .AddSingleton<ICrashHandler>(_mockHandler));

        var report = await sut.GetReport(CancellationToken.None);

        Assert.Null(report);
        Assert.Equal(1, _mockReporter.GetReportCount);
        Assert.Equal(0, _mockHandler.HandleCount);
    }

    [Fact]
    public async Task ReturnsReport()
    {
        var crash = new MockCrash();
        _mockReporter.Crash = crash;
        var sut = GetSut(services => services
            .AddSingleton<ICrashReportProvider>(_mockReporter)
            .AddSingleton<ICrashHandler>(_mockHandler));

        var report = await sut.GetReport(CancellationToken.None);

        Assert.Same(crash, report);
        Assert.Equal(1, _mockReporter.GetReportCount);
        Assert.Equal(0, _mockHandler.HandleCount);
    }

    [Fact]
    public async Task AggregatesMultipleReports()
    {
        var crashA = new MockCrash { Type = "A" };
        var crashB = new MockCrash { Type = "B" };
        var sut = GetSut(services => services
            .AddSingleton<ICrashReportProvider>(new MockCrashReporter { Crash = crashA })
            .AddSingleton<ICrashReportProvider>(new MockCrashReporter { Crash = crashB }));

        var report = await sut.GetReport(CancellationToken.None);

        var aggregate = Assert.IsType<AggregateCrash>(report);
        Assert.Equal(2, aggregate.InnerCrashes.Count);
        Assert.Contains(crashA, aggregate.InnerCrashes);
        Assert.Contains(crashB, aggregate.InnerCrashes);
    }

    [Fact]
    public async Task SkipsReporterThatThrows()
    {
        var crash = new MockCrash();
        var throwing = new MockThrowingCrashReporter();
        var sut = GetSut(services => services
            .AddSingleton<ICrashReportProvider>(throwing)
            .AddSingleton<ICrashReportProvider>(new MockCrashReporter { Crash = crash }));

        var report = await sut.GetReport(CancellationToken.None);

        Assert.Same(crash, report);
        Assert.Equal(1, throwing.GetReportCount);
    }

    [Fact]
    public async Task SkipsReporterReturningNull()
    {
        var crash = new MockCrash();
        var sut = GetSut(services => services
            .AddSingleton<ICrashReportProvider>(new MockCrashReporter()) // null crash
            .AddSingleton<ICrashReportProvider>(new MockCrashReporter { Crash = crash }));

        var report = await sut.GetReport(CancellationToken.None);

        Assert.Same(crash, report);
    }

    [Fact]
    public async Task ConcurrentCallReturnsNullWhileFirstInFlight()
    {
        var crash = new MockCrash();
        var blocking = new MockBlockingCrashReporter { Crash = crash };
        var sut = GetSut(services => services
            .AddSingleton<ICrashReportProvider>(blocking));

        var first = sut.GetReport(CancellationToken.None).AsTask();
        await blocking.Entered; // first call now holds the semaphore

        var second = await sut.GetReport(CancellationToken.None);
        Assert.Null(second); // semaphore guard rejected the concurrent call

        blocking.Release();
        Assert.Same(crash, await first);
    }

}