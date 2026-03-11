namespace TatweerPOS.Models;

public class LicenseInfo
{
    public string LicenseKey { get; set; } = "";
    public string MachineFingerprint { get; set; } = "";
    public bool IsValid { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string BranchName { get; set; } = "";
    public DateTime? ActivatedAt { get; set; }
}
