using TatweerPOS.Models;

namespace TatweerPOS.Data;

/// <summary>
/// Facade for all mock data classes.
/// Credentials match the MockCredentials table in CLAUDE.md / README.md.
/// </summary>
public static class MockData
{
    // Mock credentials (per CLAUDE.md & README.md):
    // Role          Email                       Password      PIN
    // Admin         admin@tatweer.com           Admin@123     0000
    // Manager       manager@tatweer.com         Manager@123   0000
    // Supervisor    supervisor@tatweer.com      Super@123     0000
    // Cashier       cashier@tatweer.com         Cash@123      0000
    // Pharmacist    pharma@tatweer.com          Pharma@123    0000

    public static List<Employee> Employees => MockEmployees.All;
    public static List<Product> Products => MockProducts.All;
    public static List<Customer> Customers => MockCustomers.All;
    public static List<Order> Orders => MockOrders.All;
    public static List<Branch> Branches => MockBranches.All;
    public static List<Supplier> Suppliers => MockSuppliers.All;
    public static List<Table> Tables => MockTables.All;
    public static List<Coupon> Coupons => MockCoupons.All;
    public static List<KitchenTicket> KitchenTickets => MockKitchenTickets.All;
}
