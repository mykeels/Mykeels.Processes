namespace Mykeels.Processes;

using System.Runtime.InteropServices;

public class OperatingSystem
{
    private static OSPlatform platform { get; set; } = OSPlatform.Create("none");
    public static OSPlatform GetOSPlatform()
    {
        if (platform != OSPlatform.Create("none"))
        {
            return platform;
        }
        string windir = Environment.GetEnvironmentVariable("windir") ?? String.Empty;
        if (!string.IsNullOrEmpty(windir) && windir.Contains(@"\") && Directory.Exists(windir))
        {
            platform = OSPlatform.Windows;
        }
        else if (File.Exists(@"/proc/sys/kernel/ostype"))
        {
            string osType = File.ReadAllText(@"/proc/sys/kernel/ostype");
            if (osType.StartsWith("Linux", StringComparison.OrdinalIgnoreCase))
            {
                // Note: Android gets here too
                platform = OSPlatform.Linux;
            }
            else
            {
                throw new Exception(osType);
            }
        }
        else if (File.Exists(@"/System/Library/CoreServices/SystemVersion.plist"))
        {
            // Note: iOS gets here too
            platform = OSPlatform.OSX;
        }
        else
        {
            throw new Exception("Unknown OS Platform");
        }
        return platform;
    }

    public static bool IsWindows()
    {
        return GetOSPlatform() == OSPlatform.Windows;
    }

    public static bool IsOSX()
    {
        return GetOSPlatform() == OSPlatform.OSX;
    }

    public static bool IsLinux()
    {
        return GetOSPlatform() == OSPlatform.Linux;
    }
}