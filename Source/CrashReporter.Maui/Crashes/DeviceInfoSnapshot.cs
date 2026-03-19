

namespace CrashReporter.Maui.Crashes;

public sealed record DeviceInfoSnapshot(
    string Model,
    string Manufacturer,
    string Name,
    string VersionString,
    string Platform
    )
{
    public static DeviceInfoSnapshot FromCurrent()
        => new (
            DeviceInfo.Model,
            DeviceInfo.Manufacturer,
            DeviceInfo.Name,
            DeviceInfo.VersionString,
            DeviceInfo.Platform.ToString()
        );
}