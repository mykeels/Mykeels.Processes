// See https://aka.ms/new-console-template for more information

using Mykeels.Processes;

Console.WriteLine("Hello, World!");

var process = Shell.Run(
    new List<string>() {
        @"echo hello"
    }
);
process.WaitForExit();
// await Sherlock.ListProcesses();
// await Sherlock.KillProcessAtPort(3000);