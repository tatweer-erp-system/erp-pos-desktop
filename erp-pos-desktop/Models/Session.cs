using TatweerPOS.Models.Enums;

namespace TatweerPOS.Models;

public class Session
{
    public string EmployeeId { get; set; } = "";
    public string EmployeeName { get; set; } = "";
    public EmployeeRole Role { get; set; }
    public string BranchId { get; set; } = "";
    public string BranchName { get; set; } = "";
    public DateTime LoginTime { get; set; }
    public DateTime LastActivity { get; set; }
}
