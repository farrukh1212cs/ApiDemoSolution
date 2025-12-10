using ApiDemo.gRPC.Services;
using ApiDemo.Shared.Data;
using ApiDemo.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repository
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add gRPC
builder.Services.AddGrpc();

var app = builder.Build();

// Map gRPC service
app.MapGrpcService<ProductServiceGrpc>();

// Optional health endpoint
app.MapGet("/", () => "gRPC Product Service running...");

app.Run();
