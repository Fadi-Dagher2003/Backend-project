namespace WarehouseApi.Contracts;

public record CreateSupplierRequest(
    string Name,
    string Country,
    string ContactEmail,
    string PhoneNumber
);