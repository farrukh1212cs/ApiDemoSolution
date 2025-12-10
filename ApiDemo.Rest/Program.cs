using ApiDemo.Shared.Data;
using ApiDemo.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configure PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("ApiDemo.Rest")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.MapControllers();


using var scope = app.Services.CreateScope();
var repo = scope.ServiceProvider.GetRequiredService<IProductRepository>();

if (!(await repo.GetAllAsync()).Any())
{
    await repo.AddAsync(new Product { Name = "Test Product 1", Price = 10 });
    await repo.AddAsync(new Product { Name = "Test Product 2", Price = 25 });
}

app.Run();
