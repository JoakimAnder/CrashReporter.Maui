using CrashReporter.Abstractions;

namespace CrashReporter.Maui.Crashes;

internal sealed class AppInfoSnapshotProvider : ISnapshotProvider
{
    public ValueTask<Snapshot> TakeSnapshot(CancellationToken cancellationToken)
    {
        var info = AppInfoSnapshot.FromCurrent();

        var data = new Dictionary<string, string>
        {
            [nameof(info.PackageName)] = info.PackageName,
            [nameof(info.Name)] = info.Name,
            [nameof(info.Version)] = info.Version,
            [nameof(info.Build)] = info.Build,
        };

        return new ValueTask<Snapshot>(new Snapshot("app", data));
    }
}
