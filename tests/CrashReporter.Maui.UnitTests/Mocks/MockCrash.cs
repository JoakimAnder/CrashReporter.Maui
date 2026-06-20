
using System.Runtime.ExceptionServices;
using CrashReporter.Maui.Crashes;

namespace CrashReporter.Maui.UnitTests.Mocks;

public class MockCrash : ICrash
{
    public Guid Id { get; set; }

    public string Raw { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime TimeStampUTC { get; set; } = DateTime.UtcNow;
    public ExceptionInfo? ExceptionInfo { get; set; }
    public IReadOnlyCollection<Snapshot> Snapshots { get; set; } = [];
}