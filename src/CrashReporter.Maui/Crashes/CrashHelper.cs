
namespace CrashReporter.Maui.Crashes;

public static class CrashHelper
{
    public static void HandeledCrash() => throw new Exception("Test crash was called");

    public static void NativeCrash() => NativeCrashHelper.Crash();

    private static object CrashProp => CrashProp;
    public static void RecursiveProp() => _ = CrashProp;
}