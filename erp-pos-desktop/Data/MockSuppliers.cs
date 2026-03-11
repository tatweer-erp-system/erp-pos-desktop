namespace TatweerPOS.Data;

using TatweerPOS.Models;

public static class MockSuppliers
{
    public static List<Supplier> All => new()
    {
        new Supplier
        {
            Id = "SUP001", Name = "Al-Marai Food Company", Contact = "Ahmed Al-Rajhi",
            Email = "orders@almarai.com.sa", Phone = "0112228800",
            Address = "Industrial City, Exit 17, Riyadh 14326, Saudi Arabia",
            PaymentTerms = "Net 30", TotalOrders = 245, LastOrderDate = new DateTime(2026, 3, 8),
            Website = "https://www.almarai.com", Notes = "Primary dairy and juice supplier", IsActive = true
        },
        new Supplier
        {
            Id = "SUP002", Name = "Savola Group", Contact = "Faisal Al-Turki",
            Email = "procurement@savola.com", Phone = "0126484444",
            Address = "Al-Hamra District, Jeddah 21482, Saudi Arabia",
            PaymentTerms = "Net 45", TotalOrders = 180, LastOrderDate = new DateTime(2026, 3, 5),
            Website = "https://www.savola.com", Notes = "Cooking oils, sugar, and packaged foods", IsActive = true
        },
        new Supplier
        {
            Id = "SUP003", Name = "Halwani Brothers", Contact = "Tariq Halwani",
            Email = "sales@halwanibrothers.com", Phone = "0126366555",
            Address = "Al-Sharafiyah District, Jeddah 21411, Saudi Arabia",
            PaymentTerms = "Net 30", TotalOrders = 132, LastOrderDate = new DateTime(2026, 2, 28),
            Website = "https://www.halwanibrothers.com", Notes = "Processed meats, pastries, and frozen goods", IsActive = true
        },
        new Supplier
        {
            Id = "SUP004", Name = "NADEC (National Agricultural Development Co.)", Contact = "Sultan Al-Harbi",
            Email = "supply@nadec.com.sa", Phone = "0114785500",
            Address = "Haradh Road, Riyadh 11564, Saudi Arabia",
            PaymentTerms = "Net 30", TotalOrders = 198, LastOrderDate = new DateTime(2026, 3, 10),
            Website = "https://www.nadec.com.sa", Notes = "Fresh dairy, poultry, and agricultural products", IsActive = true
        },
        new Supplier
        {
            Id = "SUP005", Name = "Al-Dawaa Medical Services", Contact = "Dr. Mona Al-Shehri",
            Email = "wholesale@al-dawaa.com", Phone = "0114629999",
            Address = "King Abdulaziz Road, Riyadh 12233, Saudi Arabia",
            PaymentTerms = "Net 60", TotalOrders = 87, LastOrderDate = new DateTime(2026, 3, 1),
            Website = "https://www.al-dawaa.com", Notes = "Pharmaceutical and personal care distributor", IsActive = true
        },
        new Supplier
        {
            Id = "SUP006", Name = "Americana Foods (KSA)", Contact = "Waleed Bin Saeed",
            Email = "ksa.orders@americana-food.com", Phone = "0112125555",
            Address = "Second Industrial City, Riyadh 14335, Saudi Arabia",
            PaymentTerms = "Net 30", TotalOrders = 165, LastOrderDate = new DateTime(2026, 3, 7),
            Website = "https://www.americana-food.com", Notes = "Frozen foods, baked goods, and restaurant supplies", IsActive = true
        },
        new Supplier
        {
            Id = "SUP007", Name = "BinDawood Wholesale", Contact = "Khaled BinDawood",
            Email = "wholesale@bindawood.com", Phone = "0122625000",
            Address = "Al-Safa District, Jeddah 21452, Saudi Arabia",
            PaymentTerms = "Net 15", TotalOrders = 210, LastOrderDate = new DateTime(2026, 3, 9),
            Website = "https://www.bindawood.com", Notes = "General grocery and household wholesale", IsActive = true
        },
        new Supplier
        {
            Id = "SUP008", Name = "Arabian Beverages Co.", Contact = "Nasser Al-Qahtani",
            Email = "orders@arabianbev.com.sa", Phone = "0132844777",
            Address = "First Industrial City, Dammam 31441, Saudi Arabia",
            PaymentTerms = "Net 30", TotalOrders = 143, LastOrderDate = new DateTime(2026, 2, 25),
            Website = "https://www.arabianbev.com.sa", Notes = "Soft drinks, water, and juice distributor", IsActive = true
        },
        new Supplier
        {
            Id = "SUP009", Name = "Gulf Electronics Trading", Contact = "Ibrahim Al-Fadl",
            Email = "sales@gulf-electronics.com", Phone = "0114563210",
            Address = "Al-Malaz District, Riyadh 12831, Saudi Arabia",
            PaymentTerms = "Net 45", TotalOrders = 56, LastOrderDate = new DateTime(2026, 2, 18),
            Website = "https://www.gulf-electronics.com", Notes = "POS hardware, peripherals, and accessories", IsActive = true
        },
        new Supplier
        {
            Id = "SUP010", Name = "Al-Safi Danone", Contact = "Reem Al-Dosari",
            Email = "trade@alsafidanone.com", Phone = "0112465588",
            Address = "Al-Kharj Road, Riyadh 14713, Saudi Arabia",
            PaymentTerms = "Net 30", TotalOrders = 176, LastOrderDate = new DateTime(2026, 3, 6),
            Website = "https://www.alsafidanone.com", Notes = "Premium dairy products and yogurt", IsActive = false
        },
    };
}
