namespace WarehouseApi.Data;

using WarehouseApi.Models;

public static class FakeWarehouseStore
{
    public static List<Product> Products { get; } = new()
    {
        new Product { Id = Guid.NewGuid(), Name = "Laptop Pro", SKU = "LAP-001", Description = "16-inch high performance", Price = 1200.00m, QuantityInStock = 15, SupplierName = "TechCorp", CreatedAt = DateTime.UtcNow.AddDays(-10) },
        new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", SKU = "MOU-002", Description = "Ergonomic optical mouse", Price = 25.50m, QuantityInStock = 120, SupplierName = "GadgetInc", CreatedAt = DateTime.UtcNow.AddDays(-9) },
        new Product { Id = Guid.NewGuid(), Name = "Mechanical Keyboard", SKU = "KEY-003", Description = "RGB back-lit switches", Price = 85.00m, QuantityInStock = 45, SupplierName = "GadgetInc", CreatedAt = DateTime.UtcNow.AddDays(-8) },
        new Product { Id = Guid.NewGuid(), Name = "Barcode Scanner", SKU = "SCN-004", Description = "USB laser scanner", Price = 150.00m, QuantityInStock = 8, SupplierName = "ScanTech", CreatedAt = DateTime.UtcNow.AddDays(-7) },
        new Product { Id = Guid.NewGuid(), Name = "Receipt Printer", SKU = "PRN-005", Description = "Thermal POS printer", Price = 220.00m, QuantityInStock = 12, SupplierName = "ScanTech", CreatedAt = DateTime.UtcNow.AddDays(-6) },
        new Product { Id = Guid.NewGuid(), Name = "27-inch Monitor", SKU = "MON-006", Description = "4K IPS display", Price = 350.00m, QuantityInStock = 20, SupplierName = "TechCorp", CreatedAt = DateTime.UtcNow.AddDays(-5) },
        new Product { Id = Guid.NewGuid(), Name = "USB-C Hub", SKU = "HUB-007", Description = "7-in-1 multi-port adapter", Price = 45.00m, QuantityInStock = 60, SupplierName = "GadgetInc", CreatedAt = DateTime.UtcNow.AddDays(-4) },
        new Product { Id = Guid.NewGuid(), Name = "Ethernet Cable 5m", SKU = "CAB-008", Description = "Cat6 snagless patch cable", Price = 10.00m, QuantityInStock = 200, SupplierName = "NetGear", CreatedAt = DateTime.UtcNow.AddDays(-3) },
        new Product { Id = Guid.NewGuid(), Name = "External SSD 1TB", SKU = "SSD-009", Description = "Portable solid state drive", Price = 110.00m, QuantityInStock = 30, SupplierName = "TechCorp", CreatedAt = DateTime.UtcNow.AddDays(-2) },
        new Product { Id = Guid.NewGuid(), Name = "Webcam HD", SKU = "CAM-010", Description = "1080p stream camera", Price = 65.00m, QuantityInStock = 25, SupplierName = "GadgetInc", CreatedAt = DateTime.UtcNow.AddDays(-1) }
    };

    public static List<Supplier> Suppliers { get; } = new()
    {
        new Supplier { Id = Guid.NewGuid(), Name = "TechCorp", Country = "USA", ContactEmail = "contact@techcorp.com", PhoneNumber = "123-456-7890" },
        new Supplier { Id = Guid.NewGuid(), Name = "GadgetInc", Country = "Canada", ContactEmail = "support@gadgetinc.ca", PhoneNumber = "987-654-3210" },
        new Supplier { Id = Guid.NewGuid(), Name = "ScanTech", Country = "Germany", ContactEmail = "info@scantech.de", PhoneNumber = "49-30-123456" }
    };
}