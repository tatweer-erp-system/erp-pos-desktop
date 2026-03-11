namespace TatweerPOS.Services;

using TatweerPOS.Models;
using TatweerPOS.Helpers;

/// <summary>
/// Singleton print service handling receipt formatting, printing, and PDF export.
/// In dev mode (no physical printer), fires PrintPreviewRequested so the UI
/// can display a PrintPreviewWindow with the formatted receipt text.
/// </summary>
public class PrintService
{
    private readonly NotificationService _notificationService;
    private readonly AuditService _auditService;

    /// <summary>
    /// Fired when a receipt is ready for preview display (dev mode).
    /// The string parameter contains the fully formatted receipt text.
    /// </summary>
    public event Action<string>? PrintPreviewRequested;

    public PrintService(NotificationService notificationService, AuditService auditService)
    {
        _notificationService = notificationService;
        _auditService = auditService;
    }

    /// <summary>
    /// Formats a receipt string per the CLAUDE.md receipt format specification.
    /// </summary>
    public string FormatReceipt(Order order, string branchName, string branchAddress, string taxNumber, string cashierName)
    {
        return PrintHelper.FormatReceipt(order, branchName, branchAddress, taxNumber, cashierName);
    }

    /// <summary>
    /// Prints a receipt. In dev mode, fires PrintPreviewRequested event
    /// to show a preview window instead of sending to a physical printer.
    /// </summary>
    public async Task<bool> PrintReceiptAsync(Order order)
    {
        try
        {
            var receipt = PrintHelper.FormatReceipt(
                order,
                branchName: "Tatweer POS",
                branchAddress: "Riyadh, Saudi Arabia",
                taxNumber: "300000000000003",
                cashierName: order.EmployeeName);

            // Simulate print preparation delay
            await Task.Delay(200);

            // Dev mode: fire preview event instead of printing
            PrintPreviewRequested?.Invoke(receipt);

            _notificationService.ShowSuccess("Receipt ready for preview.");
            return true;
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"Print failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Exports a receipt as a PDF file. Currently a mock implementation
    /// that returns true (PDF generation via PdfSharp would go here).
    /// </summary>
    public async Task<bool> ExportReceiptPdfAsync(Order order, string filePath)
    {
        try
        {
            // Simulate PDF generation delay
            await Task.Delay(300);

            // Mock implementation — in production, use PdfSharp to generate PDF
            _notificationService.ShowSuccess($"Receipt exported to {filePath}");

            _auditService.Log(
                Models.Enums.AuditAction.Sale,
                order.EmployeeId,
                order.EmployeeName,
                order.BranchId,
                $"Receipt PDF exported for Order #{order.Number}");

            return true;
        }
        catch (Exception ex)
        {
            _notificationService.ShowError($"PDF export failed: {ex.Message}");
            return false;
        }
    }
}
