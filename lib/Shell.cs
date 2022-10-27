namespace Mykeels.Processes;

using System.Diagnostics;
using System.Collections;

public class Shell
{
    public static Process Run(
        List<string> commands,
        IDictionary<string, string?>? envs = null,
        string? cwd = null,
        Action<string>? outputHandler = null
    )
    {
        if (OperatingSystem.IsWindows())
        {
            return RunInCMD(commands, envs, cwd, outputHandler);
        }
        else if (OperatingSystem.IsLinux() || OperatingSystem.IsOSX())
        {
            return RunInCMD(commands, envs, cwd, outputHandler);
        }
        throw new Exception("Unsupported OS Platform");
    }

    public static Process RunInCMD(
        List<string> commands,
        IDictionary<string, string?>? envs = null,
        string? cwd = null,
        Action<string>? outputHandler = null
    )
    {
        if (envs == null)
        {
            envs = new Dictionary<string, string?>();
        }
        if (String.IsNullOrEmpty(cwd))
        {
            cwd = System.IO.Directory.GetCurrentDirectory();
        }
        if (outputHandler == null)
        {
            outputHandler = (string line) =>
            {
                Console.WriteLine("Received: {0}", line);
            };
        }
        commands.Add("exit");

        var process = new System.Diagnostics.Process();
        process.StartInfo.FileName = "cmd";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

        if (envs != null)
        {
            foreach (string key in envs.Keys)
            {
                process.StartInfo.Environment.Add(key, envs[key]?.ToString());
            }
        }
        process.StartInfo.WorkingDirectory = cwd;
        process.OutputDataReceived += (object sender, DataReceivedEventArgs args) =>
                {
                    string line = (args.Data ?? String.Empty);
                    outputHandler(line);
                };
        process.ErrorDataReceived += (object sender, DataReceivedEventArgs args) =>
                {
                    string line = (args.Data ?? String.Empty);
                    outputHandler($"Error: {line}");
                };
        process.Start();
        foreach (string command in commands)
        {
            process.StandardInput.WriteLine(command);
        }
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return process;
    }

    public static Process RunInBash(
        List<string> commands,
        IDictionary<string, string?>? envs = null,
        string? cwd = null,
        Action<string>? outputHandler = null
    )
    {
        if (envs == null)
        {
            envs = new Dictionary<string, string?>();
        }
        if (String.IsNullOrEmpty(cwd))
        {
            cwd = System.IO.Directory.GetCurrentDirectory();
        }
        if (outputHandler == null)
        {
            outputHandler = (string line) =>
            {
                Console.WriteLine("Received: {0}", line);
            };
        }
        commands.Add("exit");

        var process = new System.Diagnostics.Process();
        process.StartInfo.FileName = "bash";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardInput = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.RedirectStandardError = true;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

        if (envs != null)
        {
            foreach (string key in envs.Keys)
            {
                process.StartInfo.Environment.Add(key, envs[key]?.ToString());
            }
        }
        process.StartInfo.WorkingDirectory = cwd;

        process.OutputDataReceived += (object sender, DataReceivedEventArgs args) =>
                {
                    string line = (args.Data ?? String.Empty);
                    outputHandler(line);
                };
        process.ErrorDataReceived += (object sender, DataReceivedEventArgs args) =>
                {
                    string line = (args.Data ?? String.Empty);
                    outputHandler($"Error: {line}");
                };
        process.Start();
        foreach (string command in commands)
        {
            process.StandardInput.WriteLine(command);
        }
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return process;
    }
}