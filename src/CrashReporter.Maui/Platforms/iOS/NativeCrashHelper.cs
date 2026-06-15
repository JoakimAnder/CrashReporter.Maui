namespace CrashReporter.Maui;

public static class NativeCrashHelper
{
    [System.Runtime.InteropServices.DllImport("__Internal")]
    static extern void abort();
    internal static void Crash()
    {
        abort();
    }
}
