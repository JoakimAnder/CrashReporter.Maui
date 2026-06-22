
namespace CrashReporter.Maui.Crashes.Android;

internal class UnhandledExceptionReporter(
    ISnapshotCollector _snapshots,
    ILogger<UnhandledExceptionReporter> _logger
    ) : ICrashReportSource
{
    private static readonly string CrashFilePath = Path.Combine(FileSystem.AppDataDirectory, "Crashes", "android_env_crash.txt");

    public Task<ICrash?> Consume(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.FromCanceled<ICrash?>(cancellationToken);

        var crash = CrashFileReader.ReadCrash<UnhandledExceptionCrash>(CrashFilePath, _logger);
        return crash;
    }

    internal ValueTask Initialize(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return ValueTask.FromCanceled(cancellationToken);

        global::Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser -= AndroidEnvironment_UnhandledExceptionRaiser;
        global::Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

        return ValueTask.CompletedTask;
    }

    private void AndroidEnvironment_UnhandledExceptionRaiser(object? sender, global::Android.Runtime.RaiseThrowableEventArgs e)
    {
        if (e.Handled)
            return;

        var crash = UnhandledExceptionCrash.FromException(e.Exception, _snapshots.GetSnapshots());
        CrashFileReader.WriteCrash(CrashFilePath, crash, _logger);
    }
}