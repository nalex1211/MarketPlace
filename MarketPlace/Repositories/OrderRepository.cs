using MarketPlace.Data;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _db;

    public OrderRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<bool> EditOrderFromVendorAsync(Orders model)
    {
        _db.Orders.Update(model);
        return await SaveChangesAsync();
    }

    public async Task<List<Orders>> GetOrdersFromVendorAsync(string vendorId)
    {
        return await _db.Orders.Include(order => order.Product).Include(x => x.DeliveryAddress)
             .Where(order => order.Product.Any(product => product.UserId == vendorId)).ToListAsync();
    }

    public async Task<List<Orders>> GetUserOrdersAsync(string userId)
    {
        return await _db.Orders.Include(x => x.Product).Include(x => x.DeliveryAddress).Where(x => x.UserId == userId).ToListAsync();
    }

    public async Task<bool> SaveChangesAsync()
    {
        var saved = await _db.SaveChangesAsync();
        return saved > 0 ? true : false;
    }
}
