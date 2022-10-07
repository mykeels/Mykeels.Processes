# Mykeels.Processes

A library for:

- Listing Running Processes using Network
- Killing a Process

## Usage

```sh
dotnet add package Mykeels.Processes
```

```cs
using Mykeels.Processes;

await Sherlock.ListProcesses();
await Sherlock.KillProcess(1234);
```