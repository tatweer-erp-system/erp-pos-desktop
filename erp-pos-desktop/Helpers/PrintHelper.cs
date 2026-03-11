using System.Text;

namespace TatweerPOS.Helpers;

using TatweerPOS.Models;

/// <summary>
/// Formats plain-text receipts and barcode labels for thermal printing.
/// Receipt layout matches the CLAUDE.md specification.
/// </summary>
public static class PrintHelper
{
    private const int ReceiptWidth = 24;
    private const string DoubleLine = "\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550";
    private const string SingleLine = "\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500";

    /// <summary>
    /// Formats a complete receipt for an order matching the CLAUDE.md receipt template.
    /// </summary>
    public static string FormatReceipt(
        Order order,
        string branchName,
        string branchAddress,
        string taxNumber,
        string cashierName,
        string? footerText = null)
    {
        var sb = new StringBuilder();

        // Header
        sb.AppendLine(DoubleLine);
        sb.AppendLine(Center("TATWEER POS"));
        sb.AppendLine(Center(branchName));
        sb.AppendLine(Center(branchAddress));
        sb.AppendLine(Center($"VAT: {taxNumber}"));
        sb.AppendLine(DoubleLine);

        // Order info
        sb.AppendLine($"Order: #{order.Number}");
        sb.AppendLine($"Date:  {order.CreatedAt:dd/MM/yyyy HH:mm}");
        sb.AppendLine($"Cashier: {cashierName}");
        sb.AppendLine(SingleLine);

        // Items
        foreach (var item in order.Items)
        {
            decimal itemTotal = item.UnitPrice * item.Quantity;
            sb.AppendLine(item.ProductName);
            sb.AppendLine($"  {item.Quantity} \u00d7 ${item.UnitPrice:F2}     ${itemTotal:F2}");
        }

        sb.AppendLine(SingleLine);

        // Totals
        sb.AppendLine(FormatAmountLine("Subtotal:", order.Subtotal));
        sb.AppendLine(FormatDiscountLine("Discount:", order.DiscountAmount));
        sb.AppendLine(FormatAmountLine("Tax (15%):", order.TaxTotal));
        sb.AppendLine(SingleLine);

        // Grand total and payment
        sb.AppendLine(FormatAmountLine("TOTAL:", order.GrandTotal));
        sb.AppendLine(FormatAmountLine("Cash:", order.AmountPaid));
        sb.AppendLine(FormatAmountLine("Change:", order.ChangeAmount));

        // Footer
        sb.AppendLine(DoubleLine);
        sb.AppendLine(Center("Thank you for visiting!"));
        if (!string.IsNullOrWhiteSpace(footerText))
            sb.AppendLine(Center(footerText));
        sb.AppendLine(DoubleLine);

        return sb.ToString();
    }

    /// <summary>
    /// Formats a simple barcode label for a product.
    /// </summary>
    public static string FormatBarcodeLabel(Product product)
    {
        var sb = new StringBuilder();
        sb.AppendLine(SingleLine);
        sb.AppendLine(product.Name);
        sb.AppendLine($"SKU: {product.Sku}");
        sb.AppendLine($"Barcode: {product.Barcode}");
        sb.AppendLine($"Price: ${product.Price:F2}");
        sb.AppendLine(SingleLine);
        return sb.ToString();
    }

    /// <summary>
    /// Centers text within the receipt width.
    /// </summary>
    private static string Center(string text)
    {
        if (text.Length >= ReceiptWidth)
            return text;

        int padding = (ReceiptWidth - text.Length) / 2;
        return new string(' ', padding) + text;
    }

    /// <summary>
    /// Formats a right-aligned amount line (e.g., "Subtotal:         $XX.XX").
    /// </summary>
    private static string FormatAmountLine(string label, decimal amount)
    {
        string amountStr = $"${amount:F2}";
        int spaces = ReceiptWidth - label.Length - amountStr.Length;
        if (spaces < 1) spaces = 1;
        return $"{label}{new string(' ', spaces)}{amountStr}";
    }

    /// <summary>
    /// Formats a discount line with a negative sign (e.g., "Discount:        -$XX.XX").
    /// </summary>
    private static string FormatDiscountLine(string label, decimal amount)
    {
        string amountStr = $"-${amount:F2}";
        int spaces = ReceiptWidth - label.Length - amountStr.Length;
        if (spaces < 1) spaces = 1;
        return $"{label}{new string(' ', spaces)}{amountStr}";
    }
}
