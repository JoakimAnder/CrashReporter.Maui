using CrashReporter.Abstractions;

namespace CrashReporter.Maui.Crashes;

internal sealed class DeviceInfoSnapshotProvider : ISnapshotProvider
{
    public Snapshot TakeSnapshot()
    {
        var info = DeviceInfoSnapshot.FromCurrent();

        var data = new Dictionary<string, string>
        {
            [nameof(info.Model)] = info.Model,
            [nameof(info.Manufacturer)] = info.Manufacturer,
            [nameof(info.Name)] = info.Name,
            [nameof(info.VersionString)] = info.VersionString,
            [nameof(info.Platform)] = info.Platform,
        };

        return new Snapshot("device", data);
    }
}
