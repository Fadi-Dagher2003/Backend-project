namespace WarehouseApi.Services;

using WarehouseApi.Contracts;
using WarehouseApi.Data;
using WarehouseApi.Models;

public interface ISupplierService
{
    IEnumerable<Supplier> GetAll();
    Supplier? GetById(Guid id);
    Supplier Create(CreateSupplierRequest request);
    bool Deactivate(Guid id);
}

public class SupplierService : ISupplierService
{
    public IEnumerable<Supplier> GetAll() => FakeWarehouseStore.Suppliers.Where(s => s.IsActive);

    public Supplier? GetById(Guid id) => FakeWarehouseStore.Suppliers.FirstOrDefault(s => s.Id == id);

    public Supplier Create(CreateSupplierRequest request)
    {
        var supplier = new Supplier
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Country = request.Country,
            ContactEmail = request.ContactEmail,
            PhoneNumber = request.PhoneNumber,
            IsActive = true
        };
        FakeWarehouseStore.Suppliers.Add(supplier);
        return supplier;
    }

    public bool Deactivate(Guid id)
    {
        var supplier = GetById(id);
        if (supplier == null) return false;

        supplier.IsActive = false;
        return true;
    }
}