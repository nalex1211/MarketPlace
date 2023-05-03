using MarketPlace.Models;
using MarketPlace.ViewModels;

namespace MarketPlace.Interfaces;

public interface IAdminRepository
{
    Task<List<ApplicationUsers>> GetAllUsersAsync();
    Task<List<Categories>> GetAllCategoriesAsync();
    Task<ApplicationUsers> GetUserByIdAsync(string id);
    Task<List<ApplicationUsers>> GetAllAdminsAsync();
    Task<List<PaymentType>> GetAllPaymentTypesAsync();
    Task<List<ShippingType>> GetAllShippingTypesAsync();
    Task<bool> DeleteUserAsync(ApplicationUsers user);
    Task<bool> DeleteCategoryAsync(Categories category);
    Task<bool> DeletePaymentTypeAsync(PaymentType type);
    Task<bool> DeleteShippingTypeAsync(ShippingType type);
    Task<bool> AddCategoryAsync(Categories category);
    Task<bool> AddPaymentTypeAsync(PaymentType type);
    Task<bool> AddShippingTypeAsync(ShippingType type);
    Task<bool> SaveAsync();
}
