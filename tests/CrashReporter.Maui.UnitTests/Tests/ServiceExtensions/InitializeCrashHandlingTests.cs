
namespace CrashReporter.Maui.UnitTests.Tests.ServiceExtensions;

public class InitializeCrashHandlingTests
{
    private MockCrashReporter _mockReporter = new();
    private MockCrashHandler _mockHandler = new();
    private IServiceCollection _services = new ServiceCollection();
    private ServiceProvider GetSut() => _services
        .AddLogging()
        .BuildServiceProvider();

    [Fact]
    public async Task InitializesAndChecksReporters()
    {
        _services.AddCrashHandling(config => {})
            .AddSingleton<ICrashReportProvider>(_mockReporter);
        var sut = GetSut();

        await sut.InitializeCrashHandling();

        Assert.Equal(1, _mockReporter.GetReportCount);
    }

    [Fact]
    public async Task WithDisabledCheckOnInit_InitializesButDoesNotCheckReporters()
    {
        _services.AddCrashHandling(config => config
                .DisableCrashCheckOnInitialization())
            .AddSingleton<ICrashReportProvider>(_mockReporter);
        var sut = GetSut();

        await sut.InitializeCrashHandling();

        Assert.Equal(0, _mockReporter.GetReportCount);
    }

    [Fact]
    public async Task InitializesAndChecksReporters_AndHandlesCrashes()
    {
        _mockReporter.Crash = new MockCrash();
        _services.AddCrashHandling(config => {})
            .AddSingleton<ICrashReportProvider>(_mockReporter)
            .AddSingleton<ICrashHandler>(_mockHandler);
        var sut = GetSut();

        await sut.InitializeCrashHandling();

        Assert.Equal(1, _mockReporter.GetReportCount);
        Assert.Equal(1, _mockHandler.HandleCount);
    }

    [Fact]
    public async Task WithManyCrashes_HandlesCrashesOnce()
    {
        _mockReporter.Crash = new MockCrash();
        _services.AddCrashHandling(config => {})
            .AddSingleton<ICrashReportProvider>(_mockReporter)
            .AddSingleton<ICrashReportProvider>(_mockReporter)
            .AddSingleton<ICrashHandler>(_mockHandler);
        var sut = GetSut();

        await sut.InitializeCrashHandling();

        Assert.Equal(2, _mockReporter.GetReportCount);
        Assert.Equal(1, _mockHandler.HandleCount);
    }

    [Fact]
    public async Task WithManyHandlers_HandlesCrashesWithAllHandlers()
    {
        _mockReporter.Crash = new MockCrash();
        _services.AddCrashHandling(config => {})
            .AddSingleton<ICrashReportProvider>(_mockReporter)
            .AddSingleton<ICrashHandler>(_mockHandler)
            .AddSingleton<ICrashHandler>(_mockHandler);
        var sut = GetSut();

        await sut.InitializeCrashHandling();

        Assert.Equal(1, _mockReporter.GetReportCount);
        Assert.Equal(2, _mockHandler.HandleCount);
    }


}