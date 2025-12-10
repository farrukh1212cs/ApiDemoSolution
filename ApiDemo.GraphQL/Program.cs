using ApiDemo.GraphQL.GraphQL;
using ApiDemo.GraphQL.GraphQL.Products;
using ApiDemo.Shared.Data;
using ApiDemo.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repository (register your concrete implementation)
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add GraphQL
builder.Services
    .AddGraphQLServer()
    .AddQueryType<ProductQuery>()       // <- make sure this is added
    .AddMutationType<ProductMutation>();

var app = builder.Build();

// Configure GraphQL endpoint
app.MapGraphQL("/graphql");

// Test endpoint
app.MapGet("/", () => "GraphQL API is running...");

app.Run();
