using System.IO;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TatweerPOS.Services;
using TatweerPOS.ViewModels;
using TatweerPOS.Views;

namespace TatweerPOS;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Configure Serilog
        var logPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "TatweerPOS", "logs", "log-.txt");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        Log.Information("TatweerPOS application starting up.");

        // Handle unhandled exceptions
        DispatcherUnhandledException += OnDispatcherUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

        // Build DI container
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        // Show login window
        var loginWindow = new LoginWindow();
        loginWindow.Show();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Services (singletons)
        services.AddSingleton<AuditService>();
        services.AddSingleton<AuthService>();
        services.AddSingleton<NavigationService>();
        services.AddSingleton<NotificationService>();
        services.AddSingleton<CacheService>();
        services.AddSingleton<SyncService>();
        services.AddSingleton<LicenseService>();
        services.AddSingleton<PrintService>();

        // ViewModels (transient)
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<PlaceholderPageViewModel>();

        // Page ViewModels (transient)
        services.AddTransient<POSViewModel>();
        services.AddTransient<InventoryViewModel>();
        services.AddTransient<CustomersViewModel>();
        services.AddTransient<OrdersViewModel>();
        services.AddTransient<TablesViewModel>();
        services.AddTransient<KitchenViewModel>();
        services.AddTransient<EmployeesViewModel>();
        services.AddTransient<SuppliersViewModel>();
        services.AddTransient<ReportsViewModel>();
        services.AddTransient<SettingsViewModel>();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        Log.Fatal(e.Exception, "Unhandled UI exception");
        MessageBox.Show(
            $"An unexpected error occurred:\n{e.Exception.Message}",
            "TatweerPOS Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
        e.Handled = true;
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            Log.Fatal(ex, "Unhandled domain exception");
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.Information("TatweerPOS application shutting down.");
        Log.CloseAndFlush();
        base.OnExit(e);
    }
}
