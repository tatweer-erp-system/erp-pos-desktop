namespace TatweerPOS.Services;

using TatweerPOS.Models;
using TatweerPOS.Models.Enums;

/// <summary>
/// Singleton authentication service handling login, logout, PIN validation,
/// and role-based permission checks. Uses hardcoded mock credentials per CLAUDE.md.
/// </summary>
public class AuthService
{
    private readonly AuditService _auditService;

    /// <summary>
    /// Mock credential entries matching CLAUDE.md specifications.
    /// </summary>
    private static readonly List<(string Email, string Password, EmployeeRole Role, string Name, string Pin)> MockCredentials = new()
    {
        ("admin@tatweer.com",      "Admin@123",   EmployeeRole.Admin,       "Administrator", "0000"),
        ("manager@tatweer.com",    "Manager@123", EmployeeRole.Manager,     "Branch Manager","0000"),
        ("supervisor@tatweer.com", "Super@123",   EmployeeRole.Supervisor,  "Supervisor",    "0000"),
        ("cashier@tatweer.com",    "Cash@123",    EmployeeRole.Cashier,     "Cashier",       "0000"),
        ("pharma@tatweer.com",     "Pharma@123",  EmployeeRole.Pharmacist,  "Pharmacist",    "0000"),
    };

    /// <summary>
    /// Permission matrix per CLAUDE.md Role Permissions table.
    /// Key = action string, Value = set of roles that are allowed.
    /// </summary>
    private static readonly Dictionary<string, HashSet<EmployeeRole>> PermissionMatrix = new()
    {
        ["pos.sell"]            = new() { EmployeeRole.Admin, EmployeeRole.Manager, EmployeeRole.Supervisor, EmployeeRole.Cashier, EmployeeRole.Pharmacist },
        ["pos.discount_high"]   = new() { EmployeeRole.Admin, EmployeeRole.Manager },
        ["pos.void"]            = new() { EmployeeRole.Admin, EmployeeRole.Manager },
        ["pos.refund"]          = new() { EmployeeRole.Admin, EmployeeRole.Manager, EmployeeRole.Supervisor },
        ["orders.view"]         = new() { EmployeeRole.Admin, EmployeeRole.Manager, EmployeeRole.Supervisor, EmployeeRole.Cashier, EmployeeRole.Pharmacist },
        ["inventory.view"]      = new() { EmployeeRole.Admin, EmployeeRole.Manager, EmployeeRole.Supervisor, EmployeeRole.Cashier, EmployeeRole.Pharmacist },
        ["inventory.edit"]      = new() { EmployeeRole.Admin, EmployeeRole.Manager, EmployeeRole.Supervisor, EmployeeRole.Pharmacist },
        ["customers.view"]      = new() { EmployeeRole.Admin, EmployeeRole.Manager, EmployeeRole.Supervisor, EmployeeRole.Cashier, EmployeeRole.Pharmacist },
        ["customers.edit"]      = new() { EmployeeRole.Admin, EmployeeRole.Manager, EmployeeRole.Supervisor },
        ["employees.view"]      = new() { EmployeeRole.Admin, EmployeeRole.Manager },
        ["employees.edit"]      = new() { EmployeeRole.Admin, EmployeeRole.Manager },
        ["reports"]             = new() { EmployeeRole.Admin, EmployeeRole.Manager, EmployeeRole.Supervisor },
        ["settings"]            = new() { EmployeeRole.Admin, EmployeeRole.Manager },
        ["audit_log"]           = new() { EmployeeRole.Admin, EmployeeRole.Manager },
    };

    public Session? CurrentSession { get; private set; }

    public event Action<Session?>? SessionChanged;

    public AuthService(AuditService auditService)
    {
        _auditService = auditService;
    }

    /// <summary>
    /// Authenticates a user against mock credentials.
    /// On success, creates a Session and logs the login action.
    /// </summary>
    public async Task<(bool Success, string? Error)> LoginAsync(string email, string password, string branchId)
    {
        // Simulate network delay
        await Task.Delay(800);

        var match = MockCredentials.FirstOrDefault(c =>
            c.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && c.Password == password);

        if (match == default)
        {
            return (false, "Invalid email or password.");
        }

        CurrentSession = new Session
        {
            EmployeeId = Guid.NewGuid().ToString("N")[..8],
            EmployeeName = match.Name,
            Role = match.Role,
            BranchId = branchId,
            BranchName = $"Branch {branchId}",
            LoginTime = DateTime.UtcNow,
            LastActivity = DateTime.UtcNow
        };

        _auditService.Log(
            AuditAction.Login,
            CurrentSession.EmployeeId,
            CurrentSession.EmployeeName,
            CurrentSession.BranchId,
            $"User {match.Email} logged in as {match.Role}");

        SessionChanged?.Invoke(CurrentSession);
        return (true, null);
    }

    /// <summary>
    /// Clears the current session and logs the logout action.
    /// </summary>
    public void Logout()
    {
        if (CurrentSession != null)
        {
            _auditService.Log(
                AuditAction.Logout,
                CurrentSession.EmployeeId,
                CurrentSession.EmployeeName,
                CurrentSession.BranchId,
                $"User {CurrentSession.EmployeeName} logged out");
        }

        CurrentSession = null;
        SessionChanged?.Invoke(null);
    }

    /// <summary>
    /// Validates a PIN against the mock PIN (0000 for all users per CLAUDE.md).
    /// </summary>
    public bool ValidatePin(string pin)
    {
        return pin == "0000";
    }

    /// <summary>
    /// Checks whether the current session user has one of the specified roles.
    /// </summary>
    public bool IsInRole(params EmployeeRole[] roles)
    {
        if (CurrentSession == null) return false;
        return roles.Contains(CurrentSession.Role);
    }

    /// <summary>
    /// Checks whether the current session user has permission for a specific action
    /// based on the permission matrix from CLAUDE.md.
    /// </summary>
    public bool HasPermission(string action)
    {
        if (CurrentSession == null) return false;

        if (PermissionMatrix.TryGetValue(action, out var allowedRoles))
        {
            return allowedRoles.Contains(CurrentSession.Role);
        }

        // Unknown action — deny by default
        return false;
    }

    /// <summary>
    /// Updates the last activity timestamp on the current session.
    /// Used to track inactivity for auto-lock.
    /// </summary>
    public void UpdateActivity()
    {
        if (CurrentSession != null)
        {
            CurrentSession.LastActivity = DateTime.UtcNow;
        }
    }
}
