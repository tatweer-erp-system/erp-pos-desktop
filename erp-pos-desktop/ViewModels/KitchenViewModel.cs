namespace TatweerPOS.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TatweerPOS.Models;
using TatweerPOS.Models.Enums;
using TatweerPOS.Data;
using TatweerPOS.Services;

public partial class KitchenViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly NotificationService _notificationService;
    private readonly AuditService _auditService;
    private readonly DispatcherTimer _autoRefreshTimer;

    // ─── Observable Properties ─────────────────────────────────────

    [ObservableProperty] private ObservableCollection<KitchenTicket> tickets = new();
    [ObservableProperty] private ObservableCollection<KitchenTicket> newTickets = new();
    [ObservableProperty] private ObservableCollection<KitchenTicket> inProgressTickets = new();
    [ObservableProperty] private ObservableCollection<KitchenTicket> readyTickets = new();
    [ObservableProperty] private KitchenTicket? selectedTicket;
    [ObservableProperty] private bool isLoading;
    [ObservableProperty] private bool autoRefreshEnabled = true;
    [ObservableProperty] private int refreshInterval = 30;
    [ObservableProperty] private int newCount;
    [ObservableProperty] private int inProgressCount;
    [ObservableProperty] private int readyCount;

    // ─── Constructor ───────────────────────────────────────────────

    public KitchenViewModel(AuthService authService, NotificationService notificationService, AuditService auditService)
    {
        _authService = authService;
        _notificationService = notificationService;
        _auditService = auditService;

        _autoRefreshTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(RefreshInterval)
        };
        _autoRefreshTimer.Tick += async (_, _) => await RefreshAsync();
        _autoRefreshTimer.Start();

        LoadTickets();
    }

    // ─── Partial property change handlers ──────────────────────────

    partial void OnAutoRefreshEnabledChanged(bool value)
    {
        if (value)
            _autoRefreshTimer.Start();
        else
            _autoRefreshTimer.Stop();
    }

    partial void OnRefreshIntervalChanged(int value)
    {
        if (value < 5) value = 5;
        _autoRefreshTimer.Interval = TimeSpan.FromSeconds(value);
    }

    // ─── Commands ──────────────────────────────────────────────────

    [RelayCommand]
    private void StartPreparing(KitchenTicket ticket)
    {
        if (ticket.Status != KitchenTicketStatus.New) return;

        ticket.Status = KitchenTicketStatus.Preparing;
        LogAudit(AuditAction.Sale, $"Kitchen ticket {ticket.OrderNumber} started preparing");
        _notificationService.ShowInfo($"Ticket {ticket.OrderNumber} is now being prepared.");
        RebuildFilteredCollections();
    }

    [RelayCommand]
    private void MarkReady(KitchenTicket ticket)
    {
        if (ticket.Status != KitchenTicketStatus.Preparing) return;

        ticket.Status = KitchenTicketStatus.Ready;
        ticket.PreparedAt = DateTime.UtcNow;
        LogAudit(AuditAction.Sale, $"Kitchen ticket {ticket.OrderNumber} marked as ready");
        _notificationService.ShowSuccess($"Ticket {ticket.OrderNumber} is ready for pickup!");
        RebuildFilteredCollections();
    }

    [RelayCommand]
    private void MarkServed(KitchenTicket ticket)
    {
        if (ticket.Status != KitchenTicketStatus.Ready) return;

        ticket.Status = KitchenTicketStatus.Served;
        LogAudit(AuditAction.Sale, $"Kitchen ticket {ticket.OrderNumber} served");
        _notificationService.ShowSuccess($"Ticket {ticket.OrderNumber} has been served.");
        RebuildFilteredCollections();
    }

    [RelayCommand]
    private void CancelTicket(KitchenTicket ticket)
    {
        if (!_authService.HasPermission("pos.void"))
        {
            _notificationService.ShowError("You do not have permission to cancel kitchen tickets. Manager approval required.");
            return;
        }

        // Remove from the collection (simulating cancellation)
        Tickets.Remove(ticket);
        LogAudit(AuditAction.Void, $"Kitchen ticket {ticket.OrderNumber} cancelled by {_authService.CurrentSession?.EmployeeName}");
        _notificationService.ShowWarning($"Ticket {ticket.OrderNumber} has been cancelled.");
        RebuildFilteredCollections();
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        IsLoading = true;

        try
        {
            await Task.Delay(300); // Simulate data fetch
            LoadTickets();
            _notificationService.ShowInfo("Kitchen display refreshed.");
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void ToggleAutoRefresh()
    {
        AutoRefreshEnabled = !AutoRefreshEnabled;
        _notificationService.ShowInfo(AutoRefreshEnabled
            ? $"Auto-refresh enabled (every {RefreshInterval}s)."
            : "Auto-refresh disabled.");
    }

    // ─── Private Helpers ───────────────────────────────────────────

    private void LoadTickets()
    {
        var all = MockKitchenTickets.All;

        Tickets = new ObservableCollection<KitchenTicket>(all);
        RebuildFilteredCollections();
    }

    private void RebuildFilteredCollections()
    {
        NewTickets = new ObservableCollection<KitchenTicket>(
            Tickets.Where(t => t.Status == KitchenTicketStatus.New)
                   .OrderBy(t => t.CreatedAt));

        InProgressTickets = new ObservableCollection<KitchenTicket>(
            Tickets.Where(t => t.Status == KitchenTicketStatus.Preparing)
                   .OrderBy(t => t.CreatedAt));

        ReadyTickets = new ObservableCollection<KitchenTicket>(
            Tickets.Where(t => t.Status == KitchenTicketStatus.Ready)
                   .OrderByDescending(t => t.PreparedAt));

        NewCount = NewTickets.Count;
        InProgressCount = InProgressTickets.Count;
        ReadyCount = ReadyTickets.Count;
    }

    private void LogAudit(AuditAction action, string description)
    {
        var session = _authService.CurrentSession;
        _auditService.Log(
            action,
            session?.EmployeeId ?? "unknown",
            session?.EmployeeName ?? "Unknown",
            session?.BranchId ?? "",
            description);
    }
}
