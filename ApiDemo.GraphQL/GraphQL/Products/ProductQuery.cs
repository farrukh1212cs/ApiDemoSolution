using ApiDemo.Shared.Repositories;

namespace ApiDemo.GraphQL.GraphQL.Products;

public class ProductQuery
{
    public async Task<IEnumerable<Product>> GetProducts(
        [Service] IProductRepository productRepository)
    {
        return await productRepository.GetAllAsync();
    }

    public async Task<Product?> GetProduct(
        Guid id,
        [Service] IProductRepository productRepository)
    {
        return await productRepository.GetByIdAsync(id);
    }

}
