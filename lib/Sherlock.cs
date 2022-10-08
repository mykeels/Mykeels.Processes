namespace Mykeels.Processes;

using System.Diagnostics;
using System.Text.RegularExpressions;

public class Sherlock
{
    public async static Task<List<RunningProcess>> ListProcesses()
    {
        var completion = new TaskCompletionSource<List<RunningProcess>>();
        var processes = new List<RunningProcess>();

        return await Task.Run(() =>
        {
            if (OperatingSystem.IsWindows())
            {
                var process = Shell.Run(
                    new List<string> {
                        $"netstat -nao"
                    },
                    outputHandler: (string line) =>
                    {
                        if (line.Contains("TCP") || line.Contains("UDP"))
                        {
                            var chunks = new Regex(@"\s+").Replace(line, " ").Trim().Split(" ");
                            string protocol = chunks[0];
                            string localAddress = chunks[1];
                            string remoteAddress = chunks[2];
                            string? connectionState = protocol == "TCP" ? chunks[3] : null;
                            string processId = protocol == "TCP" ? chunks[4] : chunks[3];
                            var runningProcess = new RunningProcess()
                            {
                                Protocol = protocol == "TCP" ? NetworkProtocol.TCP : NetworkProtocol.UDP,
                                ConnectionState = connectionState,
                                LocalAddress = localAddress,
                                RemoteAddress = remoteAddress,
                                ProcessId = Convert.ToInt32(processId)
                            };
                            processes.Add(
                                runningProcess
                            );
                            Console.WriteLine(runningProcess.Serialize());
                        }
                    }
                );
                process.WaitForExit();
            }
            else if (OperatingSystem.IsLinux() || OperatingSystem.IsOSX())
            {
                var process = Shell.Run(
                    new List<string> {
                        $"lsof -Pi"
                    },
                    outputHandler: (string line) =>
                    {
                        if (line.Contains("TCP") || line.Contains("UDP"))
                        {
                            var chunks = new Regex(@"\s+").Replace(line, " ").Trim().Split(" ");
                            string processId = chunks[2];
                            string protocol = chunks[7];
                            string address = chunks[8];
                            string localAddress = address.Split("->")[0];
                            string remoteAddress = address.Split("->")[1].Split(" ")[0];
                            string? connectionState = address.Split("->")[1].Split(" ")[1].Replace("(", "").Replace(")", "");
                            var runningProcess = new RunningProcess()
                            {
                                Protocol = protocol == "TCP" ? NetworkProtocol.TCP : NetworkProtocol.UDP,
                                ConnectionState = connectionState,
                                LocalAddress = localAddress,
                                RemoteAddress = remoteAddress,
                                ProcessId = Convert.ToInt32(processId)
                            };
                            processes.Add(
                                runningProcess
                            );
                            Console.WriteLine(runningProcess.Serialize());
                        }
                    }
                );
                process.WaitForExit();
            }
            return processes;
        });
    }

    public async static Task KillProcess(int processId)
    {
        await Task.Run(() =>
        {
            if (OperatingSystem.IsWindows())
            {
                var process = Shell.Run(
                    new List<string> {
                        $"TaskKill /F /PID {processId}"
                    }
                );
                process.WaitForExit();
            }
            else if (OperatingSystem.IsLinux() || OperatingSystem.IsOSX())
            {
                var process = Shell.Run(
                    new List<string> {
                        $"kill -9 {processId}"
                    }
                );
                process.WaitForExit();
            }
        });
    }

    public async static Task KillProcessAtPort(int portId)
    {
        var processes = await ListProcesses();
        var process = processes.Where(p => p.LocalAddress?.EndsWith($":{portId}") ?? false).First();
        if (process != null) {
            await KillProcess(process.ProcessId);
        }
    }
}

