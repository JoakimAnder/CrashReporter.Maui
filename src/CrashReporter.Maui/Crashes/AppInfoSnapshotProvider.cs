
namespace CrashReporter.Maui.Crashes;

internal sealed class AppInfoSnapshotProvider : ISnapshotProvider
{
    public Snapshot TakeSnapshot()
    {
        var info = AppInfoSnapshot.FromCurrent();

        var data = new Dictionary<string, string>
        {
            [nameof(info.PackageName)] = info.PackageName,
            [nameof(info.Name)] = info.Name,
            [nameof(info.Version)] = info.Version,
            [nameof(info.Build)] = info.Build,
        };

        return new Snapshot("app", data);
    }
}
