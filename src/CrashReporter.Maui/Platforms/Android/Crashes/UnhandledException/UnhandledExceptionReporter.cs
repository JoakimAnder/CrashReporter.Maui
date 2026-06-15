
namespace CrashReporter.Maui.Crashes.Android;

internal class UnhandledExceptionReporter(
    ISnapshotCollector _snapshots,
    ILogger<UnhandledExceptionReporter> _logger
    ) : ICrashReporter
{
    private static readonly string CrashFilePath = Path.Combine(FileSystem.AppDataDirectory, "Crashes", "android_env_crash.txt");

    public Task<ICrash?> GetReport(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.FromCanceled<ICrash?>(cancellationToken);

        var crash = CrashFileReader.ReadCrash<UnhandledExceptionCrash>(CrashFilePath, _logger);
        return Task.FromResult(crash);
    }

    public Task Initialize(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.FromCanceled(cancellationToken);

        AndroidEnvironment.UnhandledExceptionRaiser -= AndroidEnvironment_UnhandledExceptionRaiser;
        AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

        return Task.CompletedTask;
    }

    private void AndroidEnvironment_UnhandledExceptionRaiser(object? sender, RaiseThrowableEventArgs e)
    {
        if (e.Handled)
            return;

        var crash = UnhandledExceptionCrash.FromException(e.Exception, _snapshots.Current);
        CrashFileReader.WriteCrash(CrashFilePath, crash, _logger);
    }
}