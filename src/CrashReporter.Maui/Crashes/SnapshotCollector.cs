using CrashReporter.Abstractions;

namespace CrashReporter.Maui.Crashes;

/// <summary>
/// Collects snapshots from all registered <see cref="ISnapshotProvider"/>s and caches the
/// latest result so it can be read synchronously from a crash handler.
/// </summary>
internal interface ISnapshotCollector
{
    /// <summary>The most recently captured snapshots. Safe to read while handling a crash.</summary>
    IReadOnlyCollection<Snapshot> Current { get; }

    /// <summary>Runs every provider and replaces <see cref="Current"/> with the result.</summary>
    ValueTask RefreshAsync(CancellationToken cancellationToken);
}

internal sealed class SnapshotCollector(
    IEnumerable<ISnapshotProvider> providers,
    ILogger<SnapshotCollector> logger
    ) : ISnapshotCollector
{
    // Resolve the providers once so we never touch the container from inside a crash handler.
    private readonly ISnapshotProvider[] _providers = providers as ISnapshotProvider[] ?? providers.ToArray();
    private volatile IReadOnlyCollection<Snapshot> _current = [];

    public IReadOnlyCollection<Snapshot> Current => _current;

    public async ValueTask RefreshAsync(CancellationToken cancellationToken)
    {
        var snapshots = new List<Snapshot>(_providers.Length);

        foreach (var provider in _providers)
        {
            try
            {
                snapshots.Add(await provider.TakeSnapshot(cancellationToken));
            }
            catch (Exception ex)
            {
                // One bad provider must not cost us the rest of the snapshots.
                logger.LogError(ex, "Snapshot provider {Provider} failed", provider.GetType().Name);
            }
        }

        _current = snapshots;
    }
}
