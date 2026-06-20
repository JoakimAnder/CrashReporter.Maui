

namespace CrashReporter.Maui.UnitTests.Tests.ServiceExtensions.AddCrashHandling.AddSnapshot;

public class AddSnapshotTests
{

    private static IServiceCollection GetSut() => new ServiceCollection()
        .AddLogging();
    
    [Fact]
    public void AddCrashHandling_AddCustomSnapshot_HasTheSnapshot()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling(config => config
                .DisableAppInfoSnapshot()
                .DisableDeviceInfoSnapshot()
                .AddSnapshot<MockSnapshotProvider>())
            .BuildServiceProvider();

        var snapshots = services.GetServices<ISnapshotProvider>();
        var snapshot = Assert.Single(snapshots);
        Assert.IsType<MockSnapshotProvider>(snapshot);
    }

    [Fact]
    public void AddCrashHandling_AddCustomSnapshot_IsSingleton()
    {
        var sut = GetSut();

        var services = sut.AddCrashHandling(config => config
                .DisableAppInfoSnapshot()
                .DisableDeviceInfoSnapshot()
                .AddSnapshot<MockSnapshotProvider>())
            .BuildServiceProvider();

        using var scope1 = services.CreateScope();
        var snapshots1 = scope1.ServiceProvider.GetServices<ISnapshotProvider>();
        var mockSnapshot1 = snapshots1.Single();

        using var scope2 = services.CreateScope();
        var snapshots2 = scope2.ServiceProvider.GetServices<ISnapshotProvider>();
        var mockSnapshot2 = snapshots2.Single();

        Assert.Same(mockSnapshot1, mockSnapshot2);
    }
}