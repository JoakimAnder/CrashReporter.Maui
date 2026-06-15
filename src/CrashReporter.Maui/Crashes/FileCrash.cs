
namespace CrashReporter.Maui.Crashes;

internal sealed record FileCrash(string Type, string Raw, DateTime TimeStampUTC) : ICrash
{
    public Guid Id { get; } = Guid.NewGuid();
    public ExceptionInfo? ExceptionInfo { get; }
    public IReadOnlyCollection<Snapshot> Snapshots { get; } = [];
}