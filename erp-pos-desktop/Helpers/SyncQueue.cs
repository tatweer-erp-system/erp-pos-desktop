namespace TatweerPOS.Helpers;

using TatweerPOS.Models;
using TatweerPOS.Models.Enums;

/// <summary>
/// In-memory queue for managing offline sync operations.
/// Records are queued when offline and processed when connectivity is restored.
/// Mock implementation — no file persistence.
/// </summary>
public static class SyncQueue
{
    private static readonly object _lock = new();
    private static readonly List<SyncRecord> _queue = new();

    /// <summary>
    /// Adds a sync record to the queue with Pending status.
    /// </summary>
    public static void Enqueue(SyncRecord record)
    {
        lock (_lock)
        {
            record.Status = SyncStatus.Pending;
            _queue.Add(record);
        }
    }

    /// <summary>
    /// Removes and returns the next pending record, or null if none available.
    /// </summary>
    public static SyncRecord? Dequeue()
    {
        lock (_lock)
        {
            var record = _queue.FirstOrDefault(r => r.Status == SyncStatus.Pending);
            if (record != null)
                _queue.Remove(record);
            return record;
        }
    }

    /// <summary>
    /// Returns all records with Pending or Error status.
    /// </summary>
    public static List<SyncRecord> GetPending()
    {
        lock (_lock)
        {
            return _queue
                .Where(r => r.Status == SyncStatus.Pending || r.Status == SyncStatus.Error)
                .ToList();
        }
    }

    /// <summary>
    /// Marks a record for retry by setting its status to Pending and incrementing RetryCount.
    /// </summary>
    public static void MarkForRetry(string operationId)
    {
        lock (_lock)
        {
            var record = _queue.FirstOrDefault(r => r.Id == operationId);
            if (record != null)
            {
                record.Status = SyncStatus.Pending;
                record.RetryCount++;
            }
        }
    }

    /// <summary>
    /// Marks a record as completed (Synced) and removes it from the queue.
    /// </summary>
    public static void MarkCompleted(string operationId)
    {
        lock (_lock)
        {
            var record = _queue.FirstOrDefault(r => r.Id == operationId);
            if (record != null)
            {
                record.Status = SyncStatus.Synced;
                _queue.Remove(record);
            }
        }
    }

    /// <summary>
    /// Returns the count of pending and errored records.
    /// </summary>
    public static int PendingCount
    {
        get
        {
            lock (_lock)
            {
                return _queue.Count(r => r.Status == SyncStatus.Pending || r.Status == SyncStatus.Error);
            }
        }
    }

    /// <summary>
    /// Clears all records from the queue.
    /// </summary>
    public static void Clear()
    {
        lock (_lock)
        {
            _queue.Clear();
        }
    }
}
