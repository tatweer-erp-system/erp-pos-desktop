using System.Windows;
using System.Windows.Controls;
using TatweerPOS.Helpers;

namespace TatweerPOS.Views.Pages;

public partial class ReportsPage : UserControl
{
    public ReportsPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        AnimationHelper.StaggeredEntrance(new UIElement[]
        {
            HeaderSection,
            FiltersRow,
            ContentArea,
        });
    }

    private void OnReportTabChanged(object sender, RoutedEventArgs e)
    {
        if (!IsLoaded) return;

        // Hide all views
        SalesReportView.Visibility = Visibility.Collapsed;
        ProductsReportView.Visibility = Visibility.Collapsed;
        CustomersReportView.Visibility = Visibility.Collapsed;
        EmployeesReportView.Visibility = Visibility.Collapsed;
        AuditLogView.Visibility = Visibility.Collapsed;

        // Show selected view
        if (TabSales.IsChecked == true)
            SalesReportView.Visibility = Visibility.Visible;
        else if (TabProducts.IsChecked == true)
            ProductsReportView.Visibility = Visibility.Visible;
        else if (TabCustomers.IsChecked == true)
            CustomersReportView.Visibility = Visibility.Visible;
        else if (TabEmployees.IsChecked == true)
            EmployeesReportView.Visibility = Visibility.Visible;
        else if (TabAudit.IsChecked == true)
            AuditLogView.Visibility = Visibility.Visible;
    }

    private void OnGenerateReportClick(object sender, RoutedEventArgs e)
    {
        // Generate report with selected date range
    }

    private void OnExportPdfClick(object sender, RoutedEventArgs e)
    {
        // Export current report to PDF
    }

    private void OnPrintClick(object sender, RoutedEventArgs e)
    {
        // Print current report
    }

    private void OnAuditFilterChanged(object sender, SelectionChangedEventArgs e)
    {
        // Filter audit log by action type
    }
}
