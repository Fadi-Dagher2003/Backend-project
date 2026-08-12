using System;
using System.Collections.Generic;

namespace Backend_project.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public int? ExpiryYear { get; set; }

    public int? SupplierId { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
