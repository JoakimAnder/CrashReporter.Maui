using CrashReporter.Maui.Configurations;
using Microsoft.Extensions.DependencyInjection;

namespace CrashReporter.Maui.UnitTests.Tests.Configurations;

public class ConfgurationTests
{
    private ServiceCollection _services = new ServiceCollection();
    private InternalConfig _config = new();

    private Configuration GetSut() => new Configuration(_services, _config);


    [Fact]
    public void DefaultConfig_HasEverythingEnabled()
    {
        var sut = GetSut();
        
        Assert.True(_config.IsCheckOnInitializationEnabled);

        Assert.True(_config.IsAndroidEnvironmentUnhandledExceptionReporterEnabled);
        Assert.True(_config.IsAndroidThreadUncaughtExceptionReporterEnabled);
        Assert.True(_config.IsPLCrashReporterEnabled);
        Assert.True(_config.IsTerminatingUnhandledExceptionReporterEnabled);
    }

    [Fact]
    public void Disabling_AllReporters_DisablesAll()
    {
        var sut = GetSut();
        sut.ConfigureReporters(config => config.DisableAll());

        Assert.False(_config.IsAndroidEnvironmentUnhandledExceptionReporterEnabled);
        Assert.False(_config.IsAndroidThreadUncaughtExceptionReporterEnabled);
        Assert.False(_config.IsPLCrashReporterEnabled);
        Assert.False(_config.IsTerminatingUnhandledExceptionReporterEnabled);
    }

    [Fact]
    public void Disabling_AllAndroidReporters_DisablesThem()
    {
        var sut = GetSut();
        sut.ConfigureReporters(config => config.Android.DisableAll());

        Assert.False(_config.IsAndroidEnvironmentUnhandledExceptionReporterEnabled);
        Assert.False(_config.IsAndroidThreadUncaughtExceptionReporterEnabled);
    }

    [Fact]
    public void Disabling_AllIOSReporters_DisablesThem()
    {
        var sut = GetSut();
        sut.ConfigureReporters(config => config.IOS.DisableAll());

        Assert.False(_config.IsPLCrashReporterEnabled);
    }

    [Fact]
    public void Disabling_AllMauiReporters_DisablesThem()
    {
        var sut = GetSut();
        sut.ConfigureReporters(config => config.Shared.DisableAll());

        Assert.False(_config.IsTerminatingUnhandledExceptionReporterEnabled);
    }

    [Fact]
    public void Disabling_AndroidUnhandledReporter_DisablesReporter()
    {
        var sut = GetSut();
        sut.ConfigureReporters(config => config.Android.EnvironmentUnhandled(c => c.Disable()));

        Assert.False(_config.IsAndroidEnvironmentUnhandledExceptionReporterEnabled);
    }

    [Fact]
    public void Disabling_AndroidUncaughtReporter_DisablesReporter()
    {
        var sut = GetSut();
        sut.ConfigureReporters(config => config.Android.ThreadUncaught(c => c.Disable()));

        Assert.False(_config.IsAndroidThreadUncaughtExceptionReporterEnabled);
    }

    [Fact]
    public void Disabling_IOSPLCReporter_DisablesReporter()
    {
        var sut = GetSut();
        sut.ConfigureReporters(config => config.IOS.PLCrashReporter(c => c.Disable()));

        Assert.False(_config.IsPLCrashReporterEnabled);
    }

    [Fact]
    public void Disabling_MauiTerminatingUnhandledReporter_DisablesReporter()
    {
        var sut = GetSut();
        sut.ConfigureReporters(config => config.Shared.TerminatingUnhandled(c => c.Disable()));

        Assert.False(_config.IsTerminatingUnhandledExceptionReporterEnabled);
    }

    [Fact]
    public void Disabling_CheckOnInit_DisablesIt()
    {
        var sut = GetSut();
        sut.DisableCrashCheckOnInitialization();

        Assert.False(_config.IsCheckOnInitializationEnabled);
    }
}