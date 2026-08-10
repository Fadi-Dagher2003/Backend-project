namespace WarehouseApi.Contracts;

public record CreateProductRequest(
    string Name,
    string SKU,
    string Description,
    decimal Price,
    int QuantityInStock,
    string SupplierName,
    DateTime? ExpiryDate
);

public record UpdateProductQuantityRequest(int QuantityInStock);

public record UpdateProductPriceRequest(decimal Price);