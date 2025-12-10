using ApiDemo.Shared.Repositories;

namespace ApiDemo.GraphQL.GraphQL.Products;

public class ProductMutation
{
    public async Task<Product> CreateProduct(
    Product input,
    [Service] IProductRepository productRepository)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = input.Name,
            Price = input.Price,
            Description = input.Description
        };

        await productRepository.AddAsync(product);

        return product;
    }

    public async Task<Product?> UpdateProduct(
        Guid id,
        Product input,
        [Service] IProductRepository productRepository)
    {
        var existing = await productRepository.GetByIdAsync(id);
        if (existing == null)
            return null;

        existing.Name = input.Name;
        existing.Price = input.Price;
        existing.Description = input.Description;

        await productRepository.UpdateAsync(existing);

        return existing;
    }

    public async Task<bool> DeleteProduct(
        Guid id,
        [Service] IProductRepository productRepository)
    {
        var existing = await productRepository.GetByIdAsync(id);
        if (existing == null)
            return false;

        await productRepository.DeleteAsync(id);
        return true;
    }
}
