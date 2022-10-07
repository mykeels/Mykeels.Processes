namespace Mykeels.Processes;

using System.Runtime.InteropServices;

public class OperatingSystem
{
    public static bool IsWindows()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }

    public static bool IsOSX()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }
    
    public static bool IsLinux()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
    }
    
    public static bool IsFreeBSD()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
    }
}