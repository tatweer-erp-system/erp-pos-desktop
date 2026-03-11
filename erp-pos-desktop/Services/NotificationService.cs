namespace TatweerPOS.Services;

/// <summary>
/// Singleton notification service for displaying toast-style messages.
/// Stores the last 50 notifications and fires events for the UI to consume.
/// </summary>
public class NotificationService
{
    private const int MaxNotifications = 50;
    private readonly List<NotificationMessage> _notifications = new();
    private readonly object _lock = new();

    /// <summary>
    /// Fired when a new notification is raised. The UI layer subscribes
    /// to this event to display toast overlays.
    /// </summary>
    public event Action<NotificationMessage>? NotificationReceived;

    /// <summary>
    /// Shows a success notification (e.g., "Order completed").
    /// </summary>
    public void ShowSuccess(string message)
    {
        RaiseNotification(NotificationType.Success, message);
    }

    /// <summary>
    /// Shows an error notification (e.g., "Payment failed").
    /// </summary>
    public void ShowError(string message)
    {
        RaiseNotification(NotificationType.Error, message);
    }

    /// <summary>
    /// Shows a warning notification (e.g., "Sync pending").
    /// </summary>
    public void ShowWarning(string message)
    {
        RaiseNotification(NotificationType.Warning, message);
    }

    /// <summary>
    /// Shows an informational notification.
    /// </summary>
    public void ShowInfo(string message)
    {
        RaiseNotification(NotificationType.Info, message);
    }

    /// <summary>
    /// Returns a copy of all stored notifications (up to 50).
    /// </summary>
    public List<NotificationMessage> GetRecentNotifications()
    {
        lock (_lock)
        {
            return new List<NotificationMessage>(_notifications);
        }
    }

    private void RaiseNotification(NotificationType type, string message)
    {
        var notification = new NotificationMessage
        {
            Type = type,
            Message = message,
            Timestamp = DateTime.UtcNow
        };

        lock (_lock)
        {
            _notifications.Add(notification);

            // Trim to keep only the last MaxNotifications entries
            if (_notifications.Count > MaxNotifications)
            {
                _notifications.RemoveAt(0);
            }
        }

        NotificationReceived?.Invoke(notification);
    }

    /// <summary>
    /// Notification type for styling purposes.
    /// </summary>
    public enum NotificationType
    {
        Success,
        Error,
        Warning,
        Info
    }

    /// <summary>
    /// Represents a single notification message with its type and timestamp.
    /// </summary>
    public class NotificationMessage
    {
        public NotificationType Type { get; set; }
        public string Message { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }
}
