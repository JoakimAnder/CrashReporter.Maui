namespace CrashReporter.Maui.Crashes.Android;

internal class UncaughtExceptionReporter(
    ISnapshotCollector _snapshots,
    ILogger<UncaughtExceptionReporter> _logger
    ) : Java.Lang.Object, Java.Lang.Thread.IUncaughtExceptionHandler, ICrashReportProvider
{
    private static readonly string CrashFilePath = Path.Combine(FileSystem.AppDataDirectory, "Crashes", "android_crash.txt");
    private Java.Lang.Thread.IUncaughtExceptionHandler? defaultHandler;

    public Task<ICrash?> GetReport(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.FromCanceled<ICrash?>(cancellationToken);

        var crash = CrashFileReader.ReadCrash<UncaughtExceptionCrash>(CrashFilePath, _logger);
        return crash;
    }

    internal ValueTask Initialize(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return ValueTask.FromCanceled(cancellationToken);

        if (Java.Lang.Thread.DefaultUncaughtExceptionHandler == this)
            return ValueTask.CompletedTask;
        
        defaultHandler = Java.Lang.Thread.DefaultUncaughtExceptionHandler;
        Java.Lang.Thread.DefaultUncaughtExceptionHandler = this;

        return ValueTask.CompletedTask;
    }
    
    public void UncaughtException(Java.Lang.Thread t, Java.Lang.Throwable e)
    {
        var crash = UncaughtExceptionCrash.FromThrowable(e, _snapshots.GetSnapshots());
        CrashFileReader.WriteCrash(CrashFilePath, crash, _logger);

        defaultHandler?.UncaughtException(t, e);
    }

}