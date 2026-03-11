using TatweerPOS.Models.Enums;

namespace TatweerPOS.Models;

public class Employee
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public EmployeeRole Role { get; set; }
    public string BranchId { get; set; } = "";
    public string Pin { get; set; } = "0000";
    public string PasswordHash { get; set; } = "";
    public string AvatarColor { get; set; } = "";
    public bool IsActive { get; set; }
    public DateTime HireDate { get; set; }
}
