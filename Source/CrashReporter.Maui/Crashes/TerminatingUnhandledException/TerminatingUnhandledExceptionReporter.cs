using System.Text.Json;

namespace CrashReporter.Maui.Crashes.Shared;

internal sealed class TerminatingUnhandledExceptionReporter(
    ILogger<TerminatingUnhandledExceptionReporter> logger
    ) : ICrashReporter
{
    private static readonly string CrashFilePath = Path.Combine(FileSystem.AppDataDirectory, "Crashes", "maui_crash.txt");

    public Task<ICrash?> GetReport(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.FromCanceled<ICrash?>(cancellationToken);

        var crash = CrashFileReader.ReadCrash<TerminatingUnhandledExceptionCrash>(CrashFilePath, logger);
        return crash;
    }

    public Task Initialize(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return Task.FromCanceled(cancellationToken);

        
        AppDomain.CurrentDomain.UnhandledException -= CurrentDomainOnUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

        return Task.CompletedTask;
    }

    private void CurrentDomainOnUnhandledException(object? sender, UnhandledExceptionEventArgs e)
    {
        if (!e.IsTerminating)
            return;

        var ex = e.ExceptionObject as Exception ?? new Exception($"Unknown error crashed the app. ExceptionObject: {e.ExceptionObject}");
        var crash = TerminatingUnhandledExceptionCrash.FromException(ex);
        CrashFileReader.WriteCrash(CrashFilePath, crash, logger);
    }
}