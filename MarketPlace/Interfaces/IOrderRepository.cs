using MarketPlace.Models;

namespace MarketPlace.Interfaces;

public interface IOrderRepository
{
    Task<List<Orders>> GetUserOrdersAsync(string userId);
    Task<List<Orders>> GetOrdersFromVendorAsync(string vendorId);
    Task<bool> EditOrderFromVendorAsync(Orders model);
    Task<bool> SaveChangesAsync();
}
