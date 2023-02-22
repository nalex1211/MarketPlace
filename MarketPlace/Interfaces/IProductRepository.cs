using MarketPlace.Models;

namespace MarketPlace.Interfaces;

public interface IProductRepository
{
    Task<List<Products>> GetAllProductsAndCategories();
    public Task<Products> GetProductByIdAsync(int id);
}
