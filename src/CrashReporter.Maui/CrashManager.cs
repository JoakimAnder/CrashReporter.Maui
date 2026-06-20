using System.Runtime.CompilerServices;

namespace CrashReporter.Maui;
public interface ICrashManager
{
    /// <summary>
    /// Get a crash report, if any exist.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask<ICrash?> GetReport(CancellationToken cancellationToken);

    /// <summary>
    /// Sends the crash report to the crash handlers.
    /// </summary>
    /// <param name="report"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    ValueTask HandleCrash(ICrash report, CancellationToken cancellationToken);
}

internal sealed class CrashManager(
    IServiceProvider serviceProvider,
    ILogger<CrashManager> logger
) : ICrashManager, IDisposable
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    public async ValueTask<ICrash?> GetReport(CancellationToken cancellationToken)
    {
        if (!await _semaphoreSlim.WaitAsync(millisecondsTimeout: 0, cancellationToken))
            return null;

        try {
            var crashes = await GetCrashes(cancellationToken).ToArrayAsync(cancellationToken);
            
            if (crashes.Length == 0)
                return null;

            if (crashes.Length == 1)
                return crashes[0];

            return new AggregateCrash(crashes);
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    private async IAsyncEnumerable<ICrash> GetCrashes([EnumeratorCancellation]CancellationToken cancellationToken)
    {
        var reporters = GetReporters();
        foreach (var reporter in reporters)
        {
            ICrash? report = null;
            try
            {
                report = await reporter.GetReport(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to get report from {CrashReporter}", reporter.GetType().Name);
            }

            if (report is null)
                continue;

            yield return report;
        }
    }

    public async ValueTask HandleCrash(ICrash report, CancellationToken cancellationToken)
    {
        if (report is null)
            return;
            
        var handlers = GetHandlers();
        foreach (var handler in handlers)
        {
            try
            {
                await handler.HandleCrash(report, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to handle crash with {CrashHandler}", handler.GetType().Name);
            }
        }
    }

    private IEnumerable<ICrashHandler> GetHandlers()
    {
        try
        {
            return serviceProvider.GetServices<ICrashHandler>();
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Failed to get crash handlers");
            return [];
        }
    }
    private IEnumerable<ICrashReportProvider> GetReporters()
    {
        try
        {
            return serviceProvider.GetServices<ICrashReportProvider>();
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Failed to get crash reporters");
            return [];
        }
    }

    public void Dispose()
    {
        _semaphoreSlim.Dispose();
    }
}