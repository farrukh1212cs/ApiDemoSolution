using ApiDemo.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiDemo.Shared.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;

    public ProductRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Product product)
    {
        product.Id = Guid.NewGuid();
        await _db.Products.AddAsync(product);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product != null)
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Product>> GetAllAsync() => await _db.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(Guid id) => await _db.Products.FindAsync(id);

    public async Task UpdateAsync(Product product)
    {
        _db.Products.Update(product);
        await _db.SaveChangesAsync();
    }
}
