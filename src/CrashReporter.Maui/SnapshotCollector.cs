using CrashReporter.Abstractions;

namespace CrashReporter.Maui;

/// <summary>
/// Collects snapshots from all registered <see cref="ISnapshotProvider"/> implementatiions.
/// </summary>
public interface ISnapshotCollector
{
    /// <summary>
    /// Gets the collected snapshots.
    /// </summary>
    /// <returns></returns>
    IReadOnlyCollection<Snapshot> GetSnapshots();
}

internal sealed class SnapshotCollector(
    IEnumerable<ISnapshotProvider> providers,
    ILogger<SnapshotCollector> logger
    ) : ISnapshotCollector
{
    // Resolve the providers once so we never again touch the container.
    private readonly ISnapshotProvider[] _providers = providers as ISnapshotProvider[] ?? [.. providers];

    public IReadOnlyCollection<Snapshot> GetSnapshots()
    {
        var snapshots = new List<Snapshot>(_providers.Length);

        foreach (var provider in _providers)
        {
            try
            {
                snapshots.Add(provider.TakeSnapshot());
            }
            catch (Exception ex)
            {
                // One bad provider must not cost us the rest of the snapshots.
                logger.LogError(ex, "Snapshot provider {Provider} failed", provider.GetType().Name);
            }
        }

        return snapshots;
    }
}
