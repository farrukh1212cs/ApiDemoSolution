using ApiDemo.Shared.Repositories;
using Grpc.Core;
using GrpcProductService.Protos;

namespace ApiDemo.gRPC.Services;

public class ProductServiceGrpc : ProductService.ProductServiceBase
{
    private readonly IProductRepository _repository;

    public ProductServiceGrpc(IProductRepository repository)
    {
        _repository = repository;
    }

    public override async Task<ProductMessage> GetProduct(ProductRequest request, ServerCallContext context)
    {
        var product = await _repository.GetByIdAsync(Guid.Parse(request.Id));
        if (product == null) throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));

        return new ProductMessage
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Description = product.Description,
            Price = (double)product.Price,
            Stock = product.Stock
        };
    }

    public override async Task<ProductList> GetProducts(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
    {
        var products = await _repository.GetAllAsync();
        var response = new ProductList();
        response.Products.AddRange(products.Select(p => new ProductMessage
        {
            Id = p.Id.ToString(),
            Name = p.Name,
            Description = p.Description,
            Price = (double)p.Price,
            Stock = p.Stock
        }));
        return response;
    }

    public override async Task<ProductMessage> CreateProduct(CreateProductRequest request, ServerCallContext context)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = (decimal)request.Price,
            Stock = request.Stock
        };
        await _repository.AddAsync(product);

        return new ProductMessage
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Description = product.Description,
            Price = (double)product.Price,
            Stock = product.Stock
        };
    }

    public override async Task<ProductMessage> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
    {
        var product = await _repository.GetByIdAsync(Guid.Parse(request.Id));
        if (product == null) throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = (decimal)request.Price;
        product.Stock = request.Stock;

        await _repository.UpdateAsync(product);

        return new ProductMessage
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Description = product.Description,
            Price = (double)product.Price,
            Stock = product.Stock
        };
    }

    public override async Task<Google.Protobuf.WellKnownTypes.BoolValue> DeleteProduct(ProductRequest request, ServerCallContext context)
    {
        var product = await _repository.GetByIdAsync(Guid.Parse(request.Id));
        if (product == null) return new Google.Protobuf.WellKnownTypes.BoolValue { Value = false };

        await _repository.DeleteAsync(product.Id);
        return new Google.Protobuf.WellKnownTypes.BoolValue { Value = true };
    }
}
