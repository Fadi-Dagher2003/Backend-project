namespace WarehouseApi.Services;

using WarehouseApi.Contracts;
using WarehouseApi.Data;
using WarehouseApi.Models;

public interface IProductService
{
    IEnumerable<Product> GetAll(bool? onlyAvailable);
    Product? GetById(Guid id);
    IEnumerable<Product> Search(string? name, string? supplier);
    bool SkuExists(string sku);
    Product Create(CreateProductRequest request);
    bool UpdateQuantity(Guid id, int quantity);
    bool UpdatePrice(Guid id, decimal price);
    bool AssignImage(Guid id, ProductImage image);
    bool SoftDelete(Guid id);
    bool AssignSupplier(Guid productId, Guid supplierId);
}

public class ProductService : IProductService
{
    public IEnumerable<Product> GetAll(bool? onlyAvailable)
    {
        var query = FakeWarehouseStore.Products.AsEnumerable();

        if (onlyAvailable == true)
        {
            query = query.Where(p => p.QuantityInStock > 0 && !p.IsArchived);
        }

        return query.OrderByDescending(p => p.CreatedAt);
    }

    public Product? GetById(Guid id) => 
        FakeWarehouseStore.Products.FirstOrDefault(p => p.Id == id);

    public IEnumerable<Product> Search(string? name, string? supplier)
    {
        var query = FakeWarehouseStore.Products.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(supplier))
            query = query.Where(p => p.SupplierName.Contains(supplier, StringComparison.OrdinalIgnoreCase));

        return query.OrderByDescending(p => p.CreatedAt);
    }

    public bool SkuExists(string sku) =>
        FakeWarehouseStore.Products.Any(p => p.SKU.Equals(sku, StringComparison.OrdinalIgnoreCase));

    public Product Create(CreateProductRequest request)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            SKU = request.SKU,
            Description = request.Description,
            Price = request.Price,
            QuantityInStock = request.QuantityInStock,
            SupplierName = request.SupplierName,
            ExpiryDate = request.ExpiryDate,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow,
            IsArchived = false
        };

        FakeWarehouseStore.Products.Add(product);
        return product;
    }

    public bool UpdateQuantity(Guid id, int quantity)
    {
        var product = GetById(id);
        if (product == null || quantity < 0) return false;

        product.QuantityInStock = quantity;
        product.LastUpdatedAt = DateTime.UtcNow;
        return true;
    }

    public bool UpdatePrice(Guid id, decimal price)
    {
        var product = GetById(id);
        if (product == null || price <= 0) return false;

        product.Price = price;
        product.LastUpdatedAt = DateTime.UtcNow;
        return true;
    }

    public bool AssignImage(Guid id, ProductImage image)
    {
        var product = GetById(id);
        if (product == null) return false;

        product.Images.Add(image);
        product.LastUpdatedAt = DateTime.UtcNow;
        return true;
    }

    public bool SoftDelete(Guid id)
    {
        var product = GetById(id);
        if (product == null) return false;

        product.IsArchived = true;
        product.LastUpdatedAt = DateTime.UtcNow;
        return true;
    }

    public bool AssignSupplier(Guid productId, Guid supplierId)
    {
        var product = GetById(productId);
        var supplier = FakeWarehouseStore.Suppliers.FirstOrDefault(s => s.Id == supplierId);

        if (product == null || supplier == null || product.IsArchived || !supplier.IsActive)
            return false;

        product.SupplierId = supplier.Id;
        product.SupplierName = supplier.Name;
        product.LastUpdatedAt = DateTime.UtcNow;
        return true;
    }
}