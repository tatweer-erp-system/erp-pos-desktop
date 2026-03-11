namespace TatweerPOS.Data;

using TatweerPOS.Models;
using TatweerPOS.Models.Enums;

public static class MockEmployees
{
    public static List<Employee> All => new()
    {
        // Branch 1 — Main Branch
        new Employee
        {
            Id = "E001", Name = "Sara Ahmed", Email = "admin@tatweer.com", Phone = "0551234001",
            Role = EmployeeRole.Admin, BranchId = "1", Pin = "0000", PasswordHash = "Admin@123",
            AvatarColor = "#6C63FF", IsActive = true, HireDate = new DateTime(2022, 1, 15)
        },
        new Employee
        {
            Id = "E002", Name = "Omar Hassan", Email = "manager@tatweer.com", Phone = "0551234002",
            Role = EmployeeRole.Manager, BranchId = "1", Pin = "0000", PasswordHash = "Manager@123",
            AvatarColor = "#00BFA6", IsActive = true, HireDate = new DateTime(2022, 3, 10)
        },
        new Employee
        {
            Id = "E003", Name = "Nora Salem", Email = "supervisor@tatweer.com", Phone = "0551234003",
            Role = EmployeeRole.Supervisor, BranchId = "1", Pin = "0000", PasswordHash = "Super@123",
            AvatarColor = "#F59E0B", IsActive = true, HireDate = new DateTime(2022, 6, 1)
        },
        new Employee
        {
            Id = "E004", Name = "Yusuf Ali", Email = "cashier@tatweer.com", Phone = "0551234004",
            Role = EmployeeRole.Cashier, BranchId = "1", Pin = "0000", PasswordHash = "Cash@123",
            AvatarColor = "#EF4444", IsActive = true, HireDate = new DateTime(2023, 1, 20)
        },
        new Employee
        {
            Id = "E005", Name = "Khalid Nasser", Email = "khalid@tatweer.com", Phone = "0551234005",
            Role = EmployeeRole.Cashier, BranchId = "1", Pin = "0000", PasswordHash = "Cash@123",
            AvatarColor = "#8B5CF6", IsActive = true, HireDate = new DateTime(2023, 4, 5)
        },
        new Employee
        {
            Id = "E006", Name = "Reem Faisal", Email = "reem@tatweer.com", Phone = "0551234006",
            Role = EmployeeRole.Cashier, BranchId = "1", Pin = "0000", PasswordHash = "Cash@123",
            AvatarColor = "#EC4899", IsActive = true, HireDate = new DateTime(2023, 7, 12)
        },
        new Employee
        {
            Id = "E007", Name = "Dr. Amira Khalil", Email = "pharma@tatweer.com", Phone = "0551234007",
            Role = EmployeeRole.Pharmacist, BranchId = "1", Pin = "0000", PasswordHash = "Pharma@123",
            AvatarColor = "#10B981", IsActive = true, HireDate = new DateTime(2022, 9, 1)
        },

        // Branch 2 — North Branch
        new Employee
        {
            Id = "E008", Name = "Fahad Al-Otaibi", Email = "fahad@tatweer.com", Phone = "0551234008",
            Role = EmployeeRole.Manager, BranchId = "2", Pin = "0000", PasswordHash = "Manager@123",
            AvatarColor = "#3B82F6", IsActive = true, HireDate = new DateTime(2022, 5, 18)
        },
        new Employee
        {
            Id = "E009", Name = "Layla Mahmoud", Email = "layla@tatweer.com", Phone = "0551234009",
            Role = EmployeeRole.Cashier, BranchId = "2", Pin = "0000", PasswordHash = "Cash@123",
            AvatarColor = "#F97316", IsActive = true, HireDate = new DateTime(2023, 9, 3)
        },
        new Employee
        {
            Id = "E010", Name = "Tariq Zayed", Email = "tariq@tatweer.com", Phone = "0551234010",
            Role = EmployeeRole.Cashier, BranchId = "2", Pin = "0000", PasswordHash = "Cash@123",
            AvatarColor = "#14B8A6", IsActive = false, HireDate = new DateTime(2023, 11, 15)
        },
    };
}
