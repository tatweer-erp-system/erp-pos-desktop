namespace TatweerPOS.Services;

using System.Windows.Threading;
using TatweerPOS.Models;
using TatweerPOS.Models.Enums;
using TatweerPOS.Helpers;

/// <summary>
/// Singleton sync service managing an offline-first sync queue.
/// Enqueues local mutations as SyncRecords, and periodically attempts
/// to sync them (simulated). Per CLAUDE.md, every data mutation must
/// go through the sync queue.
/// </summary>
public class SyncService
{
    private readonly AuditService _auditService;
    private readonly NotificationService _notificationService;
    private readonly List<SyncRecord> _queue = new();
    private readonly object _lock = new();
    private readonly DispatcherTimer _autoSyncTimer;

    private SyncStatus _currentStatus = SyncStatus.Synced;

    public SyncStatus CurrentStatus
    {
        get => _currentStatus;
        private set
        {
            if (_currentStatus != value)
            {
                _currentStatus = value;
                StatusChanged?.Invoke(value);
            }
        }
    }

    public int PendingChanges
    {
        get
        {
            lock (_lock)
            {
                return _queue.Count(r => r.Status == SyncStatus.Pending);
            }
        }
    }

    public DateTime? LastSyncTime { get; private set; }

    public event Action<SyncStatus>? StatusChanged;

    public SyncService(AuditService auditService, NotificationService notificationService)
    {
        _auditService = auditService;
        _notificationService = notificationService;

        // Auto-sync every 30 seconds
        _autoSyncTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(30)
        };
        _autoSyncTimer.Tick += async (_, _) => await AutoSyncAsync();
        _autoSyncTimer.Start();
    }

    /// <summary>
    /// Manually triggers a sync operation. Simulates a 1-second network delay,
    /// then marks all pending records as synced.
    /// </summary>
    public async Task SyncNowAsync()
    {
        if (PendingChanges == 0)
        {
            CurrentStatus = SyncStatus.Synced;
            return;
        }

        CurrentStatus = SyncStatus.Pending;

        try
        {
            // Simulate network sync
            await Task.Delay(1000);

            lock (_lock)
            {
                foreach (var record in _queue.Where(r => r.Status == SyncStatus.Pending))
                {
                    record.Status = SyncStatus.Synced;
                }
            }

            LastSyncTime = DateTime.UtcNow;
            CurrentStatus = SyncStatus.Synced;

            _notificationService.ShowSuccess("Sync completed successfully.");
            _auditService.Log(
                AuditAction.SyncSuccess,
                "system",
                "SyncService",
                "",
                $"Synced all pending changes at {LastSyncTime:HH:mm:ss}");
        }
        catch (Exception ex)
        {
            CurrentStatus = SyncStatus.Error;
            _notificationService.ShowError($"Sync failed: {ex.Message}");
            _auditService.Log(
                AuditAction.SyncFailure,
                "system",
                "SyncService",
                "",
                $"Sync failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Enqueues a new change to be synced with the server.
    /// Per CLAUDE.md, every data mutation must go through this queue.
    /// </summary>
    public void EnqueueChange(SyncType type, string entity, string entityId, string payload)
    {
        var record = new SyncRecord
        {
            Id = Guid.NewGuid().ToString("N"),
            Type = type,
            Entity = entity,
            EntityId = entityId,
            Payload = payload,
            Timestamp = DateTime.UtcNow,
            DeviceId = HardwareFingerprint.Generate(),
            Status = SyncStatus.Pending,
            RetryCount = 0,
            LastError = null
        };

        lock (_lock)
        {
            _queue.Add(record);
        }

        CurrentStatus = SyncStatus.Pending;
    }

    /// <summary>
    /// Clears all records from the sync queue.
    /// </summary>
    public void ClearQueue()
    {
        lock (_lock)
        {
            _queue.Clear();
        }

        CurrentStatus = SyncStatus.Synced;
    }

    /// <summary>
    /// Returns a copy of all sync records for inspection.
    /// </summary>
    public List<SyncRecord> GetQueue()
    {
        lock (_lock)
        {
            return new List<SyncRecord>(_queue);
        }
    }

    private async Task AutoSyncAsync()
    {
        if (PendingChanges > 0)
        {
            await SyncNowAsync();
        }
    }
}
