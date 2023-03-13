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

    public bool Add(Products model)
    {
        _db.Add(model);
        return Save();
    }

    public async Task<List<Categories>> GetAllCategoriesAsync()
    {
        return await _db.Categories.Include(x => x.Products).ToListAsync();
    }

    public async Task<List<Products>> GetAllProductsAndCategories()
    {
        return await _db.Products.Include(product => product.Category).ToListAsync();
    }

    public async Task<Products> GetProductByIdAsync(int id)
    {
        return await _db.Products.Include(product => product.Category).FirstOrDefaultAsync(x => x.Id == id);
    }
    public bool Save()
    {
        var saved = _db.SaveChanges();
        return saved > 0 ? true : false;
    }
}
