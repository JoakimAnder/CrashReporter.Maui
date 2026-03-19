
namespace CrashReporter.Maui.Crashes;

public sealed record ExceptionInfo(
    string Type,
    string Message,
    string? StackTrace,
    ExceptionInfo? InnerException,
    IReadOnlyList<ExceptionInfo>? InnerExceptions  // for AggregateException
)
{
    public static ExceptionInfo FromException(Exception ex) => new(
        Type: ex.GetType().FullName ?? ex.GetType().Name,
        Message: ex.Message,
        StackTrace: ex.StackTrace,
        InnerException: ex.InnerException is not null ? FromException(ex.InnerException) : null,
        InnerExceptions: ex is AggregateException agg ? agg.InnerExceptions.Select(FromException).ToList() : null
    );
}