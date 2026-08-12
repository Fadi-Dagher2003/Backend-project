using Microsoft.EntityFrameworkCore;
using WarehouseApi.Models;
using WarehouseApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Register AppDbContext with PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Add Controllers and Swagger/OpenAPI support
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. Register your services as Scoped (since they use AppDbContext)
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

var app = builder.Build();

// 4. Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // This powers the /swagger dashboard page
}

app.UseHttpsRedirection();

app.UseAuthorization();

// 5. Map your API controllers to routes 
app.MapControllers();

app.Run();