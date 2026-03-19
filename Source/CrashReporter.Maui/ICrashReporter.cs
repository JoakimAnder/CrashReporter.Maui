using CrashReporter.Maui.Crashes;

namespace CrashReporter.Maui;

public interface ICrashReporter
{
    Task<ICrash?> GetReport(CancellationToken cancellationToken);
    Task Initialize(CancellationToken cancellationToken);
}
