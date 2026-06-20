namespace CrashReporter.Maui.ExampleApp.Services;

internal sealed class CustomSnapshotProvider : ISnapshotProvider
{
    private static string _lastVsitedPage = "None";
    private static int _lastCount = 0;

    // The implementation of TakeSnapshot should be fast and not throw exceptions, since it will be called while handling a crash.
    // Prefer to store any necessary state in fields and just return that state here, rather than doing complex calculations or accessing services.
    public async ValueTask<IReadOnlyDictionary<string, string>> TakeSnapshot(CancellationToken cancellationToken)
    {
        var data = new Dictionary<string, string>
        {
            ["CustomKey"] = "CustomValue",
            ["LastVisitedPage"] = _lastVsitedPage,
            ["LastCount"] = _lastCount.ToString(),
        };

        return new ValueTask<IReadOnlyDictionary<string, string>>(data);
    }

    internal static void RefreshPage(string page)
    {
        _lastVsitedPage = page;
    }
    
    internal static void RefreshCount(int count)
    {
        _lastCount = count;
    }
}