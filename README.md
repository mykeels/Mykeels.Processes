# Mykeels.Processes

A library for:

- Listing Processes
- Killing a Process

## Usage

```cs
using Mykeels.Processes;

const processes = await Sherlock.ListProcesses();

// to kill a process running at port 3000
const culpritProcess = processes.Find(p => p.LocalAddress?.EndsWith($":3000") ?? false);
if (culpritProcess != null) {
    await Sherlock.KillProcess();
}

// or
await Sherlock.KillProcessByPort(3000);
```