using TatweerPOS.Models.Enums;

namespace TatweerPOS.Models;

public class SyncRecord
{
    public string Id { get; set; } = "";
    public SyncType Type { get; set; }
    public string Entity { get; set; } = "";
    public string EntityId { get; set; } = "";
    public string Payload { get; set; } = "";
    public DateTime Timestamp { get; set; }
    public string DeviceId { get; set; } = "";
    public SyncStatus Status { get; set; }
    public int RetryCount { get; set; } = 0;
    public string? LastError { get; set; }
}
