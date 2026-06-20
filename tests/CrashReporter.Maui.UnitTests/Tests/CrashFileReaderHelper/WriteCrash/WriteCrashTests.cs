
using CrashReporter.Maui.Crashes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CrashReporter.Maui.UnitTests.Tests.CrashFileReaderHelper.WriteCrash;

public sealed class WriteCrashTests : IDisposable
{
    private readonly string _dir = Path.Combine(
        Path.GetTempPath(), "CrashReporterTests", Guid.NewGuid().ToString("N"));
    private readonly ILogger _logger = NullLogger.Instance;

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    [Fact]
    public void CreatesMissingDirectory()
    {
        // Nested path whose parent directory does not exist yet.
        var path = Path.Combine(_dir, "nested", "sub", "crash.json");

        CrashFileReader.WriteCrash(path, new MockCrash { Type = "Boom" }, _logger);

        Assert.True(File.Exists(path));
    }

    [Fact]
    public void WritesSerializedCrashContent()
    {
        var path = Path.Combine(_dir, "crash.json");

        CrashFileReader.WriteCrash(path, new MockCrash { Type = "Boom" }, _logger);

        var text = File.ReadAllText(path);
        Assert.Contains("Boom", text);
    }
}
