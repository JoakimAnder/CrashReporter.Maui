using System.Text.Json;

namespace CrashReporter.Maui.Crashes.Shared;

internal sealed class TerminatingUnhandledExceptionReporter(
    ISnapshotCollector snapshots,
    ILogger<TerminatingUnhandledExceptionReporter> logger
    ) : ICrashReportSource
{
    private static readonly string CrashFilePath = Path.Combine(FileSystem.AppDataDirectory, "Crashes", "maui_crash.txt");

    public Task<ICrash?> Consume(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.FromCanceled<ICrash?>(cancellationToken);

        var crash = CrashFileReader.ReadCrash<TerminatingUnhandledExceptionCrash>(CrashFilePath, logger);
        return crash;
    }

    internal ValueTask Initialize(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return ValueTask.FromCanceled(cancellationToken);
        
        AppDomain.CurrentDomain.UnhandledException -= CurrentDomainOnUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

        return ValueTask.CompletedTask;
    }

    private void CurrentDomainOnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
    {
        if (!e.IsTerminating)
            return;

        var ex = e.ExceptionObject as Exception ?? new Exception($"Unknown error crashed the app. ExceptionObject: {e.ExceptionObject}");
        var crash = TerminatingUnhandledExceptionCrash.FromException(ex, snapshots.GetSnapshots());
        CrashFileReader.WriteCrash(CrashFilePath, crash, logger);
    }
}