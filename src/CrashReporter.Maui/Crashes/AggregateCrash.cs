
namespace CrashReporter.Maui.Crashes;

public sealed class AggregateCrash(IReadOnlyCollection<ICrash> innerCrashes) : ICrash
{
    public string Type => nameof(AggregateCrash);
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime TimeStampUTC { get; } = DateTime.UtcNow;
    public ExceptionInfo? ExceptionInfo { get; }
    public IReadOnlyCollection<Snapshot> Snapshots { get; } = [];

    public string Raw { get; } = "Multiple crashes reported, se inner reports";

    public IReadOnlyCollection<ICrash> InnerCrashes { get; } = innerCrashes;
}