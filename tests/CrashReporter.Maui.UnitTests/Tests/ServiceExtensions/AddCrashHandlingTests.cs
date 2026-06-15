
using CrashReporter.Maui.Crashes;
using Microsoft.Extensions.DependencyInjection;

namespace CrashReporter.Maui.UnitTests.Tests.ServiceExtensions;

public class AddCrashHandlingTests
{
    private IServiceCollection GetSut() => new ServiceCollection()
        .AddLogging();

    [Fact]
    public void AddCrashHandling_DefaultsTo_HavingReporters()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling().BuildServiceProvider();

        var reporters = services.GetServices<ICrashReporter>();
        Assert.NotEmpty(reporters);
    }

    [Fact]
    public void AddCrashHandling_DisableAllReporters_HasNoReporters()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling(config => config.ConfigureReporters(c => c.DisableAll()))
            .BuildServiceProvider();

        var reporters = services.GetServices<ICrashReporter>();
        Assert.Empty(reporters);
    }

    [Fact]
    public void AddCrashHandling_AddCustomReporter_HasTheReporter()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling(config => config
                .ConfigureReporters(c => c.DisableAll())
                .AddReporter<MockCrashReporter>())
            .BuildServiceProvider();

        var reporters = services.GetServices<ICrashReporter>();
        var reporter = Assert.Single(reporters);
        Assert.IsType<MockCrashReporter>(reporter);
    }

    [Fact]
    public void AddCrashHandling_AddCustomReporter_IsSingleton()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling(config => config
                .ConfigureReporters(c => c.DisableAll())
                .AddReporter<MockCrashReporter>())
            .BuildServiceProvider();

        using var scope1 = services.CreateScope();
        var reporters1 = scope1.ServiceProvider.GetServices<ICrashReporter>();
        var mockReporter1 = reporters1.Single();

        using var scope2 = services.CreateScope();
        var reporters2 = scope2.ServiceProvider.GetServices<ICrashReporter>();
        var mockReporter2 = reporters2.Single();

        Assert.Same(mockReporter1, mockReporter2);
    }

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