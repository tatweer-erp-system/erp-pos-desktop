using TatweerPOS.Models.Enums;

namespace TatweerPOS.Models;

public class AuditLog
{
    public string Id { get; set; } = "";
    public AuditAction Action { get; set; }
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
    public string BranchId { get; set; } = "";
    public string Description { get; set; } = "";
    public DateTime Timestamp { get; set; }
    public string DeviceId { get; set; } = "";
}
