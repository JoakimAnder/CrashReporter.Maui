
using CrashReporter.Maui.Crashes;
using Microsoft.Extensions.DependencyInjection;

namespace CrashReporter.Maui.UnitTests.Tests.ServiceExtensions;

public class InitializeCrashHandlingTests
{
    private MockCrashReporter _mockReporter = new();
    private MockCrashHandler _mockHandler = new();
    private IServiceCollection _services = new ServiceCollection();
    private IServiceProvider GetSut() => _services
        .BuildServiceProvider();

    public async Task InitializeCrashHandling_InitializesAndChecksReporters()
    {
        _services.AddCrashHandling(config => config.ConfigureReporters(c => c.DisableAll()))
            .AddSingleton<ICrashReporter>(_mockReporter);
        var sut = GetSut();

        await sut.InitializeCrashHandling();

        Assert.Equal(1, _mockReporter.InitCount);
        Assert.Equal(1, _mockReporter.GetReportCount);
    }

    public async Task InitializeCrashHandling_WithDisabledCheckOnInit_InitializesButDoesNotCheckReporters()
    {
        _services.AddCrashHandling(config => config
                .ConfigureReporters(c => c.DisableAll())
                .DisableCrashCheckOnInitialization())
            .AddSingleton<ICrashReporter>(_mockReporter);
        var sut = GetSut();

        await sut.InitializeCrashHandling();

        Assert.Equal(1, _mockReporter.InitCount);
        Assert.Equal(0, _mockReporter.GetReportCount);
    }

    public async Task InitializeCrashHandling_InitializesAndChecksReporters_AndHandlesCrashes()
    {
        _mockReporter.Crash = new MockCrash();
        _services.AddCrashHandling(config => config
                .ConfigureReporters(c => c.DisableAll()))
            .AddSingleton<ICrashReporter>(_mockReporter)
            .AddSingleton<ICrashHandler>(_mockHandler);
        var sut = GetSut();

        await sut.InitializeCrashHandling();

        Assert.Equal(1, _mockReporter.InitCount);
        Assert.Equal(1, _mockReporter.GetReportCount);
        Assert.Equal(1, _mockHandler.HandleCount);
    }

    public async Task InitializeCrashHandling_WithManyCrashes_HandlesCrashesOnce()
    {
        _mockReporter.Crash = new MockCrash();
        _services.AddCrashHandling(config => config
                .ConfigureReporters(c => c.DisableAll()))
            .AddSingleton<ICrashReporter>(_mockReporter)
            .AddSingleton<ICrashReporter>(_mockReporter)
            .AddSingleton<ICrashHandler>(_mockHandler);
        var sut = GetSut();

        await sut.InitializeCrashHandling();

        Assert.Equal(2, _mockReporter.InitCount);
        Assert.Equal(2, _mockReporter.GetReportCount);
        Assert.Equal(1, _mockHandler.HandleCount);
    }
}