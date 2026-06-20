using CrashReporter.Maui.Configurations;

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

        Assert.True(_config.IsMauiTerminatingUnhandeledExceptionReporterEnabled);
        Assert.True(_config.IsAndroidUncaughtExceptionReporterEnabled);
        Assert.True(_config.IsAndroidUnhandeledExceptionReporterEnabled);

        Assert.True(_config.IsAppInfoSnapshotEnabled);
        Assert.True(_config.IsDeviceInfoSnapshotEnabled);
    }

    [Fact]
    public void Disabling_AndroidUnhandledReporter_DisablesReporter()
    {
        var sut = GetSut();
        sut.DisableAndroidUnhandeledExceptionReporter();

        Assert.False(_config.IsAndroidUnhandeledExceptionReporterEnabled);
    }

    [Fact]
    public void Disabling_AndroidUncaughtReporter_DisablesReporter()
    {
        var sut = GetSut();
        sut.DisableAndroidUncaughtExceptionReporter();

        Assert.False(_config.IsAndroidUncaughtExceptionReporterEnabled);
    }

    [Fact]
    public void Disabling_MauiTerminatingUnhandledReporter_DisablesReporter()
    {
        var sut = GetSut();
        sut.DisableMauiTerminatingUnhandeledExceptionReporter();

        Assert.False(_config.IsMauiTerminatingUnhandeledExceptionReporterEnabled);
    }

    [Fact]
    public void Disabling_AppInfoSnapshot_DisablesIt()
    {
        var sut = GetSut();
        sut.DisableAppInfoSnapshot();

        Assert.False(_config.IsAppInfoSnapshotEnabled);
    }

    [Fact]
    public void Disabling_DeviceInfoSnapshot_DisablesIt()
    {
        var sut = GetSut();
        sut.DisableDeviceInfoSnapshot();

        Assert.False(_config.IsDeviceInfoSnapshotEnabled);
    }

    [Fact]
    public void Disabling_CheckOnInit_DisablesIt()
    {
        var sut = GetSut();
        sut.DisableCrashCheckOnInitialization();

        Assert.False(_config.IsCheckOnInitializationEnabled);
    }
}