
namespace CrashReporter.Maui.Crashes;

public sealed record AppInfoSnapshot(
    string PackageName,
    string Name,
    string Version,
    string Build
)
{
    public static AppInfoSnapshot FromCurrent()
        => new(
            AppInfo.PackageName,
            AppInfo.Name,
            AppInfo.VersionString,
            AppInfo.BuildString
        );
}