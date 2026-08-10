using WarehouseApi.Services;
var builder = WebApplication.CreateBuilder(args);

// 1. Add Controllers and Swagger/OpenAPI support
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Register your services (Uncomment and match your actual service names if needed)
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<ISupplierService, SupplierService>();

var app = builder.Build();

// 3. Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // This powers the /swagger dashboard page
}

app.UseHttpsRedirection();

app.UseAuthorization();

// 4. Map your API controllers to routes 
app.MapControllers();

app.Run();