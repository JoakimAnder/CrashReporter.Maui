
namespace CrashReporter.Maui.UnitTests.Mocks;

/// <summary>A reporter that blocks inside <see cref="GetReport"/> until <see cref="Release"/> is
/// called. Lets a test hold the CrashManager semaphore open while a concurrent call is exercised.</summary>
public class MockBlockingCrashReporter : ICrashReportProvider
{
    private readonly TaskCompletionSource _entered = new();
    private readonly TaskCompletionSource _release = new();

    public ICrash? Crash { get; set; }

    /// <summary>Completes once <see cref="GetReport"/> has been entered (semaphore acquired).</summary>
    public Task Entered => _entered.Task;

    /// <summary>Allows the pending <see cref="GetReport"/> call to complete.</summary>
    public void Release() => _release.TrySetResult();

    public async Task<ICrash?> GetReport(CancellationToken cancellationToken)
    {
        _entered.TrySetResult();
        await _release.Task;
        return Crash;
    }
}
