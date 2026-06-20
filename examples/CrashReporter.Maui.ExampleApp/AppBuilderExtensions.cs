using CrashReporter.Maui.ExampleApp.Services;

namespace CrashReporter.Maui.ExampleApp;

internal static class AppBuilderExtensions
{
    public static MauiAppBuilder UseCrashHandling(this MauiAppBuilder builder)
    {
        builder.AddCrashHandling(config =>
        {
            // Make sure to add crash handler(s). These will be called when a crash is detected, and you can use them to log the crash, send it to a server, or whatever you want.
            // There are no default handlers as it's hightly dependent on what kind of app / system you're making.
            config.AddHandler<CustomCrashHandler>();

            // You're able to disable the three simple reporters that comes with the library.
            // config.DisableMauiTerminatingUnhandeledExceptionReporter();
            // config.DisableAndroidUncaughtExceptionReporter();
            // config.DisableAndroidUnhandeledExceptionReporter();

            // You can also add your own custom reporters. Just implement the ICrashReportProvider interface and add it here.
            config.AddReporter<CustomCrashReporter>();

            // By default, the library includes two snapshot providers that collects basic information about the device and the app. You can disable these if you don't need them or if you want to implement your own versions of these snapshots. 
            // config.DisableDeviceInfoSnapshot();
            // config.DisableAppInfoSnapshot();

            // You can also add custom snapshot providers to include custom snapshots in the crash report. Just implement the ISnapshotProvider interface and add it here.
            // Some examples of snapshots you might want to include are the user's last known location, the state of the app's navigation stack, or the values of certain important variables in your app.
            // Memory snapshots, Uptime snapshots, and CPU usage snapshots could be useful as well.
            config.AddSnapshot<CustomSnapshotProvider>();

            // You may disable automatic crash checking on initialization if you want to check for crashes manually. E.g. if you want to authorize the user first before showing them information about the crash or options for how to proceed.
            // config.DisableCheckOnInitialization();
        });

        return builder;
    }
}