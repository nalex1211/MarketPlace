using MarketPlace.Models;

namespace MarketPlace.Interfaces;

public interface IProductRepository
{
    Task<List<Products>> GetAllProdsAndCategsAsync();
    Task<Products> GetProductByIdAsync(int id);
    Task<List<Categories>> GetAllCategoriesAsync();
    Task<List<Products>> GetAllUserProductsAsync(string id);
    Task<Categories> GetCategoryByCellAsync(string cellValue);
    Task<List<Products>> GetCategoryProductsAsync(string category);
    Task<List<Cart>> GetCartProductsAsync(string id);
    Task<Cart> FindCartProductsAsync(int productId, string userId);
    Task<List<Products>> GetAllCartProductsAsync(int[] selectedItemIds);
    Task<List<PaymentType>> GetAllPaymentTypesAsync();
    Task<List<ShippingType>> GetAllShippingTypeTypesAsync();
    Task CreateOrderAsync(Orders order);
    Task<bool> UpdateAsync(Products model);
    Task<bool> DeleteAsync(Products model);
    Task<bool> AddAsync(Products model);
    Task<bool> AddDeliveryAddressAsync(AddressesForOrders model);
    Task<bool> AddToCartAsync(Cart model);
    Task<bool> RemoveFromCartAsync(Cart model);
    Task<bool> SaveAsync();
}
