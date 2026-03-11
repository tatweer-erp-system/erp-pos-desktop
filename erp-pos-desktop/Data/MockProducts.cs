namespace TatweerPOS.Data;

using TatweerPOS.Models;
using TatweerPOS.Models.Enums;

public static class MockProducts
{
    public static List<Product> All => new()
    {
        // ═══════════════════════════════════════════════════
        // RESTAURANT  (20 items)
        // ═══════════════════════════════════════════════════

        // Burgers
        new Product { Id = "P001", Name = "Classic Burger", Barcode = "5901234100011", Sku = "REST-BUR-001", CategoryName = "Burgers", Price = 28m, CostPrice = 12m, Stock = 100, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83c\udf54" },
        new Product { Id = "P002", Name = "Cheese Burger", Barcode = "5901234100028", Sku = "REST-BUR-002", CategoryName = "Burgers", Price = 32m, CostPrice = 14m, Stock = 90, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83e\uddc0" },
        new Product { Id = "P003", Name = "Double Burger", Barcode = "5901234100035", Sku = "REST-BUR-003", CategoryName = "Burgers", Price = 42m, CostPrice = 20m, Stock = 60, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83c\udf54" },
        new Product { Id = "P004", Name = "Chicken Burger", Barcode = "5901234100042", Sku = "REST-BUR-004", CategoryName = "Burgers", Price = 30m, CostPrice = 13m, Stock = 80, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83c\udf57" },
        new Product { Id = "P005", Name = "Veggie Burger", Barcode = "5901234100059", Sku = "REST-BUR-005", CategoryName = "Burgers", Price = 26m, CostPrice = 10m, Stock = 40, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83e\udd66" },

        // Pizza
        new Product { Id = "P006", Name = "Margherita Pizza", Barcode = "5901234100066", Sku = "REST-PIZ-001", CategoryName = "Pizza", Price = 35m, CostPrice = 15m, Stock = 50, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83c\udf55" },
        new Product { Id = "P007", Name = "Pepperoni Pizza", Barcode = "5901234100073", Sku = "REST-PIZ-002", CategoryName = "Pizza", Price = 40m, CostPrice = 18m, Stock = 55, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83c\udf55" },
        new Product { Id = "P008", Name = "BBQ Chicken Pizza", Barcode = "5901234100080", Sku = "REST-PIZ-003", CategoryName = "Pizza", Price = 45m, CostPrice = 20m, Stock = 45, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83c\udf55" },
        new Product { Id = "P009", Name = "Four Cheese Pizza", Barcode = "5901234100097", Sku = "REST-PIZ-004", CategoryName = "Pizza", Price = 42m, CostPrice = 19m, Stock = 35, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83e\uddc0" },

        // Drinks
        new Product { Id = "P010", Name = "Coca Cola", Barcode = "5901234100103", Sku = "REST-DRK-001", CategoryName = "Drinks", Price = 5m, CostPrice = 2m, Stock = 200, MinStock = 20, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83e\udd64" },
        new Product { Id = "P011", Name = "Pepsi", Barcode = "5901234100110", Sku = "REST-DRK-002", CategoryName = "Drinks", Price = 5m, CostPrice = 2m, Stock = 180, MinStock = 20, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83e\udd64" },
        new Product { Id = "P012", Name = "Orange Juice", Barcode = "5901234100127", Sku = "REST-DRK-003", CategoryName = "Drinks", Price = 12m, CostPrice = 5m, Stock = 120, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83e\uddc3" },
        new Product { Id = "P013", Name = "Water 500ml", Barcode = "5901234100134", Sku = "REST-DRK-004", CategoryName = "Drinks", Price = 3m, CostPrice = 1m, Stock = 200, MinStock = 30, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83d\udca7" },
        new Product { Id = "P014", Name = "Arabic Coffee", Barcode = "5901234100141", Sku = "REST-DRK-005", CategoryName = "Drinks", Price = 15m, CostPrice = 5m, Stock = 150, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\u2615" },
        new Product { Id = "P015", Name = "Mint Tea", Barcode = "5901234100158", Sku = "REST-DRK-006", CategoryName = "Drinks", Price = 10m, CostPrice = 3m, Stock = 140, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83c\udf75" },

        // Desserts & Sides
        new Product { Id = "P016", Name = "Chocolate Cake", Barcode = "5901234100165", Sku = "REST-DES-001", CategoryName = "Desserts", Price = 22m, CostPrice = 9m, Stock = 30, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83c\udf70" },
        new Product { Id = "P017", Name = "French Fries", Barcode = "5901234100172", Sku = "REST-SID-001", CategoryName = "Sides", Price = 12m, CostPrice = 4m, Stock = 100, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83c\udf5f" },
        new Product { Id = "P018", Name = "Chicken Nuggets", Barcode = "5901234100189", Sku = "REST-SID-002", CategoryName = "Sides", Price = 18m, CostPrice = 7m, Stock = 80, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83c\udf57" },
        new Product { Id = "P019", Name = "Pancakes", Barcode = "5901234100196", Sku = "REST-BRK-001", CategoryName = "Breakfast", Price = 20m, CostPrice = 8m, Stock = 50, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83e\udd5e" },
        new Product { Id = "P020", Name = "Eggs Benedict", Barcode = "5901234100202", Sku = "REST-BRK-002", CategoryName = "Breakfast", Price = 25m, CostPrice = 10m, Stock = 40, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Restaurant, Emoji = "\ud83c\udf73" },

        // ═══════════════════════════════════════════════════
        // RETAIL  (10 items)
        // ═══════════════════════════════════════════════════

        new Product { Id = "P021", Name = "Wireless Mouse", Barcode = "5901234200018", Sku = "RET-ELE-001", CategoryName = "Electronics", Price = 45m, CostPrice = 22m, Stock = 60, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Retail, Emoji = "\ud83d\uddb1\ufe0f" },
        new Product { Id = "P022", Name = "USB-C Cable", Barcode = "5901234200025", Sku = "RET-ELE-002", CategoryName = "Electronics", Price = 15m, CostPrice = 5m, Stock = 150, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Retail, Emoji = "\ud83d\udd0c" },
        new Product { Id = "P023", Name = "Bluetooth Speaker", Barcode = "5901234200032", Sku = "RET-ELE-003", CategoryName = "Electronics", Price = 120m, CostPrice = 65m, Stock = 30, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Retail, Emoji = "\ud83d\udd0a" },
        new Product { Id = "P024", Name = "Power Bank 10000mAh", Barcode = "5901234200049", Sku = "RET-ELE-004", CategoryName = "Electronics", Price = 75m, CostPrice = 35m, Stock = 45, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Retail, Emoji = "\ud83d\udd0b" },
        new Product { Id = "P025", Name = "Cotton T-Shirt", Barcode = "5901234200056", Sku = "RET-CLO-001", CategoryName = "Clothing", Price = 55m, CostPrice = 20m, Stock = 100, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Retail, Emoji = "\ud83d\udc55" },
        new Product { Id = "P026", Name = "Baseball Cap", Barcode = "5901234200063", Sku = "RET-CLO-002", CategoryName = "Clothing", Price = 35m, CostPrice = 12m, Stock = 80, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Retail, Emoji = "\ud83e\udde2" },
        new Product { Id = "P027", Name = "Scented Candle Set", Barcode = "5901234200070", Sku = "RET-HOM-001", CategoryName = "Home", Price = 40m, CostPrice = 15m, Stock = 50, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Retail, Emoji = "\ud83d\udd6f\ufe0f" },
        new Product { Id = "P028", Name = "Wooden Photo Frame", Barcode = "5901234200087", Sku = "RET-HOM-002", CategoryName = "Home", Price = 30m, CostPrice = 10m, Stock = 40, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Retail, Emoji = "\ud83d\uddbc\ufe0f" },
        new Product { Id = "P029", Name = "Hand Cream", Barcode = "5901234200094", Sku = "RET-BEA-001", CategoryName = "Beauty", Price = 25m, CostPrice = 8m, Stock = 70, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Retail, Emoji = "\ud83e\uddf4" },
        new Product { Id = "P030", Name = "Lip Balm", Barcode = "5901234200100", Sku = "RET-BEA-002", CategoryName = "Beauty", Price = 12m, CostPrice = 3m, Stock = 120, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Retail, Emoji = "\ud83d\udc84" },

        // ═══════════════════════════════════════════════════
        // PHARMACY  (10 items)
        // ═══════════════════════════════════════════════════

        new Product { Id = "P031", Name = "Paracetamol 500mg", Barcode = "5901234300015", Sku = "PHA-MED-001", CategoryName = "Medications", Price = 8m, CostPrice = 3m, Stock = 200, MinStock = 20, TaxRate = 15, BusinessMode = BusinessMode.Pharmacy, Emoji = "\ud83d\udc8a", ExpiryDate = new DateTime(2027, 6, 30), BatchNumber = "PCM-2025-001" },
        new Product { Id = "P032", Name = "Ibuprofen 400mg", Barcode = "5901234300022", Sku = "PHA-MED-002", CategoryName = "Medications", Price = 12m, CostPrice = 5m, Stock = 150, MinStock = 15, TaxRate = 15, BusinessMode = BusinessMode.Pharmacy, Emoji = "\ud83d\udc8a", ExpiryDate = new DateTime(2027, 9, 15), BatchNumber = "IBU-2025-003" },
        new Product { Id = "P033", Name = "Amoxicillin 500mg", Barcode = "5901234300039", Sku = "PHA-MED-003", CategoryName = "Medications", Price = 22m, CostPrice = 10m, Stock = 80, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Pharmacy, Emoji = "\ud83d\udc8a", ExpiryDate = new DateTime(2027, 3, 20), BatchNumber = "AMX-2025-007" },
        new Product { Id = "P034", Name = "Vitamin C 1000mg", Barcode = "5901234300046", Sku = "PHA-MED-004", CategoryName = "Medications", Price = 18m, CostPrice = 7m, Stock = 120, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Pharmacy, Emoji = "\ud83c\udf4a", ExpiryDate = new DateTime(2028, 1, 10), BatchNumber = "VTC-2025-012" },
        new Product { Id = "P035", Name = "Hand Sanitizer 250ml", Barcode = "5901234300053", Sku = "PHA-PER-001", CategoryName = "Personal Care", Price = 10m, CostPrice = 4m, Stock = 180, MinStock = 15, TaxRate = 15, BusinessMode = BusinessMode.Pharmacy, Emoji = "\ud83e\uddf4", ExpiryDate = new DateTime(2027, 12, 31), BatchNumber = "SAN-2025-020" },
        new Product { Id = "P036", Name = "Face Mask Pack (50)", Barcode = "5901234300060", Sku = "PHA-PER-002", CategoryName = "Personal Care", Price = 15m, CostPrice = 6m, Stock = 100, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Pharmacy, Emoji = "\ud83d\ude37", ExpiryDate = new DateTime(2028, 6, 30), BatchNumber = "MSK-2025-005" },
        new Product { Id = "P037", Name = "Baby Wipes (80 sheets)", Barcode = "5901234300077", Sku = "PHA-BAB-001", CategoryName = "Baby", Price = 14m, CostPrice = 6m, Stock = 90, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Pharmacy, Emoji = "\ud83d\udc76", ExpiryDate = new DateTime(2028, 3, 15), BatchNumber = "BWP-2025-008" },
        new Product { Id = "P038", Name = "Baby Shampoo 200ml", Barcode = "5901234300084", Sku = "PHA-BAB-002", CategoryName = "Baby", Price = 20m, CostPrice = 9m, Stock = 60, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Pharmacy, Emoji = "\ud83d\udc76", ExpiryDate = new DateTime(2028, 8, 20), BatchNumber = "BSH-2025-002" },
        new Product { Id = "P039", Name = "Digital Thermometer", Barcode = "5901234300091", Sku = "PHA-MDC-001", CategoryName = "Medical", Price = 35m, CostPrice = 15m, Stock = 40, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Pharmacy, Emoji = "\ud83c\udf21\ufe0f", BatchNumber = "THR-2025-015" },
        new Product { Id = "P040", Name = "Blood Pressure Monitor", Barcode = "5901234300107", Sku = "PHA-MDC-002", CategoryName = "Medical", Price = 150m, CostPrice = 80m, Stock = 15, MinStock = 3, TaxRate = 15, BusinessMode = BusinessMode.Pharmacy, Emoji = "\ud83e\ude7a", BatchNumber = "BPM-2025-004" },

        // ═══════════════════════════════════════════════════
        // SUPERMARKET  (10 items)
        // ═══════════════════════════════════════════════════

        new Product { Id = "P041", Name = "Fresh Milk 1L", Barcode = "5901234400012", Sku = "SUP-DAI-001", CategoryName = "Dairy", Price = 7m, CostPrice = 4m, Stock = 100, MinStock = 15, TaxRate = 15, BusinessMode = BusinessMode.Supermarket, Emoji = "\ud83e\udd5b", ExpiryDate = new DateTime(2026, 4, 10) },
        new Product { Id = "P042", Name = "Greek Yogurt 500g", Barcode = "5901234400029", Sku = "SUP-DAI-002", CategoryName = "Dairy", Price = 9m, CostPrice = 5m, Stock = 80, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Supermarket, Emoji = "\ud83e\udd5b", ExpiryDate = new DateTime(2026, 4, 5) },
        new Product { Id = "P043", Name = "Cheddar Cheese Block", Barcode = "5901234400036", Sku = "SUP-DAI-003", CategoryName = "Dairy", Price = 18m, CostPrice = 10m, Stock = 50, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Supermarket, Emoji = "\ud83e\uddc0", ExpiryDate = new DateTime(2026, 5, 20) },
        new Product { Id = "P044", Name = "White Bread Loaf", Barcode = "5901234400043", Sku = "SUP-BAK-001", CategoryName = "Bakery", Price = 5m, CostPrice = 2m, Stock = 60, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Supermarket, Emoji = "\ud83c\udf5e" },
        new Product { Id = "P045", Name = "Butter Croissant", Barcode = "5901234400050", Sku = "SUP-BAK-002", CategoryName = "Bakery", Price = 4m, CostPrice = 2m, Stock = 70, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Supermarket, Emoji = "\ud83e\udd50" },
        new Product { Id = "P046", Name = "Banana per kg", Barcode = "5901234400067", Sku = "SUP-PRO-001", CategoryName = "Produce", Price = 6m, CostPrice = 3m, Stock = 80, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Supermarket, Unit = "kg", Emoji = "\ud83c\udf4c" },
        new Product { Id = "P047", Name = "Apple per kg", Barcode = "5901234400074", Sku = "SUP-PRO-002", CategoryName = "Produce", Price = 8m, CostPrice = 4m, Stock = 75, MinStock = 10, TaxRate = 15, BusinessMode = BusinessMode.Supermarket, Unit = "kg", Emoji = "\ud83c\udf4e" },
        new Product { Id = "P048", Name = "Frozen Pizza", Barcode = "5901234400081", Sku = "SUP-FRZ-001", CategoryName = "Frozen", Price = 15m, CostPrice = 7m, Stock = 40, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Supermarket, Emoji = "\ud83c\udf55", ExpiryDate = new DateTime(2026, 12, 31) },
        new Product { Id = "P049", Name = "Ice Cream Tub 1L", Barcode = "5901234400098", Sku = "SUP-FRZ-002", CategoryName = "Frozen", Price = 20m, CostPrice = 9m, Stock = 35, MinStock = 5, TaxRate = 15, BusinessMode = BusinessMode.Supermarket, Emoji = "\ud83c\udf68", ExpiryDate = new DateTime(2027, 3, 15) },
        new Product { Id = "P050", Name = "Water Pack (12x500ml)", Barcode = "5901234400104", Sku = "SUP-BEV-001", CategoryName = "Beverages", Price = 12m, CostPrice = 6m, Stock = 100, MinStock = 15, TaxRate = 15, BusinessMode = BusinessMode.Supermarket, Emoji = "\ud83d\udca7" },
    };
}
