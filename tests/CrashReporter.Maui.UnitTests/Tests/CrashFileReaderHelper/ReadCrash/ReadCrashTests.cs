
using CrashReporter.Maui.Crashes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace CrashReporter.Maui.UnitTests.Tests.CrashFileReaderHelper.ReadCrash;

public sealed class ReadCrashTests : IDisposable
{
    private readonly string _dir = Path.Combine(
        Path.GetTempPath(), "CrashReporterTests", Guid.NewGuid().ToString("N"));
    private readonly ILogger _logger = NullLogger.Instance;

    public ReadCrashTests() => Directory.CreateDirectory(_dir);

    public void Dispose()
    {
        if (Directory.Exists(_dir))
            Directory.Delete(_dir, recursive: true);
    }

    private string FileIn(string name) => Path.Combine(_dir, name);

    [Fact]
    public async Task MissingFileReturnsNull()
    {
        var crash = await CrashFileReader.ReadCrash<MockCrash>(FileIn("does-not-exist.json"), _logger);

        Assert.Null(crash);
    }

    [Fact]
    public async Task WriteThenReadRoundTrips()
    {
        var path = FileIn("crash.json");
        var original = new MockCrash { Type = "Boom", Raw = "raw details" };
        CrashFileReader.WriteCrash(path, original, _logger);

        var read = await CrashFileReader.ReadCrash<MockCrash>(path, _logger);

        var crash = Assert.IsType<MockCrash>(read);
        Assert.Equal(original.Type, crash.Type);
        Assert.Equal(original.Raw, crash.Raw);
    }

    [Fact]
    public async Task ReadDeletesTheFile()
    {
        var path = FileIn("crash.json");
        CrashFileReader.WriteCrash(path, new MockCrash { Type = "Boom" }, _logger);

        _ = await CrashFileReader.ReadCrash<MockCrash>(path, _logger);

        Assert.False(File.Exists(path));
    }

    [Fact]
    public async Task InvalidJsonFallsBackToFileCrash()
    {
        var path = FileIn("garbage.json");
        const string contents = "this is not json {";
        await File.WriteAllTextAsync(path, contents);

        var read = await CrashFileReader.ReadCrash<MockCrash>(path, _logger);

        var crash = Assert.IsType<FileCrash>(read);
        Assert.Equal(nameof(MockCrash), crash.Type);
        Assert.Equal(contents, crash.Raw);
    }
}
