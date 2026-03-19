namespace CrashReporter.Maui;

public static class NativeCrashHelper
{
    [System.Runtime.InteropServices.DllImport("libc")]
    static extern void abort();
    internal static void Crash()
    {
        abort();
    }
}
