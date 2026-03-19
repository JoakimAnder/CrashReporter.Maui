using CrashReporter.Maui.Crashes;

namespace CrashReporter.Maui;

/// <summary>
/// Implementations should handle the crash report.
/// Send it to a server, log it, or whatever.
/// </summary>
public interface ICrashHandler
{
    /// <summary>
    /// Handle a crash report.
    /// Send it to a server, log it, or whatever.
    /// Any exeptions will be handeled by <see cref="IErrorHandler.Handle(Exceptions.CrashHandlerException)"/>
    /// </summary>
    /// <param name="report">The reported crash</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task HandleCrash(ICrash report, CancellationToken cancellationToken);
}
