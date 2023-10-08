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

    public async Task<bool> AddAsync(Products model)
    {
        await _db.AddAsync(model);
        return await SaveAsync();
    }

    public async Task<bool> DeleteAsync(Products model)
    {
        _db.Entry(model).State = EntityState.Deleted;
        return await SaveAsync();
    }

    public async Task<List<Categories>> GetAllCategoriesAsync()
    {
        return await _db.Categories.Include(x => x.Products).ToListAsync();
    }

    public async Task<List<Products>> GetAllProdsAndCategsAsync()
    {
        return await _db.Products.Include(product => product.Category).ToListAsync();
    }

    public async Task<List<Products>> GetAllUserProductsAsync(string id)
    {
        return await _db.Products.Include(x => x.Category).Where(x => x.UserId == id).ToListAsync();
    }

    public async Task<Products> GetProductByIdAsync(int id)
    {
        return await _db.Products.Include(product => product.Category).FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<bool> SaveAsync()
    {
        var saved = await _db.SaveChangesAsync();
        return saved > 0 ? true : false;
    }

    public async Task<bool> UpdateAsync(Products model)
    {
        _db.Update(model);
        return await SaveAsync();
    }

    public async Task<Categories> GetCategoryByCellAsync(string cellValue)
    {
        return await _db.Categories.Where(x => x.Name == cellValue).FirstOrDefaultAsync();
    }

    public async Task<List<Products>> GetCategoryProductsAsync(string category)
    {
        return await _db.Products.Include(product => product.Category).Where(product => product.Category.Name == category).ToListAsync();
    }

    public async Task<List<Cart>> GetCartProductsAsync(string id)
    {
        return await _db.Carts.Include(x => x.Product).ThenInclude(x => x.Category).Where(x => x.UserId == id).ToListAsync();
    }

    public async Task<bool> AddToCartAsync(Cart model)
    {
        await _db.Carts.AddAsync(model);
        return await SaveAsync();
    }

    public async Task<bool> RemoveFromCartAsync(Cart model)
    {

        _db.Entry(model).State = EntityState.Deleted;
        return await SaveAsync();
    }

    public async Task<Cart> FindCartProductsAsync(int cartProductId, string userId)
    {
        return await _db.Carts.Where(x => x.ProductId == cartProductId && x.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task<List<Products>> GetAllCartProductsAsync(int[] selectedItemIds)
    {
        var cartProducts = new List<Products>();
        for (int i = 0; i < selectedItemIds.Length; i++)
        {
            var product = await GetProductByIdAsync(selectedItemIds[i]);
            cartProducts.Add(product);
        }
        return cartProducts;
    }

    public async Task CreateOrderAsync(Orders order)
    {
        await _db.Orders.AddAsync(order);
    }

    public async Task<List<PaymentType>> GetAllPaymentTypesAsync()
    {
        return await _db.PaymentTypes.ToListAsync();
    }

    public async Task<List<ShippingType>> GetAllShippingTypeTypesAsync()
    {
        return await _db.ShippingTypes.ToListAsync();
    }

    public async Task<bool> AddDeliveryAddressAsync(AddressesForOrders model)
    {
        await _db.AddressesForOrders.AddAsync(model);
        return await SaveAsync();
    }

    public async Task<Products> FindProductByNameAsync(string name)
    {
        return await _db.Products.Include(x=>x.Category).FirstOrDefaultAsync(x => x.Name == name);
    }

    public async Task<IEnumerable<Products>> GetProductsByNameAsync(string term)
    {
        return await _db.Products
            .Where(p => p.Name.Contains(term))
            .ToListAsync();
    }

}
