using MarketPlace.Models;

namespace MarketPlace.Interfaces;

public interface IProductRepository
{
    Task<List<Products>> GetAllProductsAndCategories();
     Task<Products> GetProductByIdAsync(int id);
    Task<List<Categories>> GetAllCategoriesAsync();
    bool Add(Products model);
    bool Save();
}
