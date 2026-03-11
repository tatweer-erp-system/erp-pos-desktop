namespace TatweerPOS.Services;

using TatweerPOS.Models;
using TatweerPOS.Models.Enums;

/// <summary>
/// Singleton audit logging service that maintains an in-memory audit trail.
/// Per CLAUDE.md, every significant action must be logged.
/// </summary>
public class AuditService
{
    private readonly List<AuditLog> _logs = new();
    private readonly object _lock = new();

    /// <summary>
    /// Records an audit log entry with the given details.
    /// Thread-safe via locking.
    /// </summary>
    public void Log(AuditAction action, string userId, string userName, string branchId, string description)
    {
        var entry = new AuditLog
        {
            Id = Guid.NewGuid().ToString("N"),
            Action = action,
            UserId = userId,
            UserName = userName,
            BranchId = branchId,
            Description = description,
            Timestamp = DateTime.UtcNow,
            DeviceId = Helpers.HardwareFingerprint.Generate()
        };

        lock (_lock)
        {
            _logs.Add(entry);
        }
    }

    /// <summary>
    /// Returns a copy of all audit log entries.
    /// </summary>
    public List<AuditLog> GetAll()
    {
        lock (_lock)
        {
            return new List<AuditLog>(_logs);
        }
    }

    /// <summary>
    /// Returns audit log entries filtered by user ID.
    /// </summary>
    public List<AuditLog> GetByUser(string userId)
    {
        lock (_lock)
        {
            return _logs.Where(l => l.UserId == userId).ToList();
        }
    }

    /// <summary>
    /// Returns audit log entries filtered by action type.
    /// </summary>
    public List<AuditLog> GetByAction(AuditAction action)
    {
        lock (_lock)
        {
            return _logs.Where(l => l.Action == action).ToList();
        }
    }

    /// <summary>
    /// Returns audit log entries within the specified date range (inclusive).
    /// </summary>
    public List<AuditLog> GetByDateRange(DateTime from, DateTime to)
    {
        lock (_lock)
        {
            return _logs.Where(l => l.Timestamp >= from && l.Timestamp <= to).ToList();
        }
    }
}
