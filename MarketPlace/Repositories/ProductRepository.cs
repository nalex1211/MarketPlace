using MarketPlace.Data;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<List<Products>> GetAllProductsAndCategories()
    {
        return await _db.Products.Include(product => product.Category).ToListAsync();
    }

    public async Task<Products> GetProductByIdAsync(int id)
    {
        return await _db.Products.Include(product => product.Category).FirstOrDefaultAsync(x => x.Id == id);
    }
}
