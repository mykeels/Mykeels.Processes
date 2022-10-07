namespace Mykeels.Processes;
public class RunningProcess
{
    public NetworkProtocol Protocol { get; set; }
    public string? LocalAddress { get; set; }
    public string? RemoteAddress { get; set; }
    public string? ConnectionState { get; set; }
    public int ProcessId { get; set; }

    public string Serialize()
    {
        return $"{ProcessId} ({Protocol}): {LocalAddress} -> {RemoteAddress}{(ConnectionState != null ? $" ({ConnectionState})" : "")}";
    }
}
