using System.Text.Json;

namespace CrashReporter.Maui.Crashes;

internal static class CrashFileReader
{
    public static Task<ICrash?> ReadCrash<TCrash>(
        string crashFilePath,
        ILogger logger
    ) where TCrash : ICrash
    {
        if (!File.Exists(crashFilePath))
            return Task.FromResult<ICrash?>(null);

        string text = string.Empty;
        DateTime crashFileCreatedUTC = DateTime.UtcNow;

        try
        {
            text = File.ReadAllText(crashFilePath);
            crashFileCreatedUTC = File.GetCreationTimeUtc(crashFilePath);
            File.Delete(crashFilePath);
        }
        catch (Exception ex)
        {
            var message = string.IsNullOrEmpty(text)
                ? "Failed to read crash report"
                : "Failed to delete crash report";
            logger.LogWarning(ex, message);
        }

        if (string.IsNullOrEmpty(text))
            return Task.FromResult<ICrash?>(null);

        ICrash? crash = null;
        try
        {
            crash = JsonSerializer.Deserialize<TCrash>(text);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to deserialize crash");
        }

        crash ??= new FileCrash(typeof(TCrash).Name, text, crashFileCreatedUTC);

        return Task.FromResult<ICrash?>(crash);
    }

    public static void WriteCrash<TCrash>(string crashFilePath, TCrash crash, ILogger logger)
        where TCrash : ICrash
    {
        try
        {
            var serializedCrash = JsonSerializer.Serialize(crash);
            Directory.CreateDirectory(Path.GetDirectoryName(crashFilePath)!);
            File.WriteAllText(crashFilePath, serializedCrash);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to write crash to file {Path}", crashFilePath);
        }
    }
}