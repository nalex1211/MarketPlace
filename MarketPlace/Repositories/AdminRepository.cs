using MarketPlace.Data;
using MarketPlace.Helpers;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;
using static MarketPlace.Helpers.Roles;

namespace MarketPlace.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly ApplicationDbContext _db;

    public AdminRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<List<ApplicationUsers>> GetAllUsersAsync()
    {
        return await _db.Users.ToListAsync();
    }

    public async Task<bool> DeleteUserAsync(ApplicationUsers user)
    {
        if (user.Address is not null)
        {
            var address = await _db.Addresses.Where(x => x.Id == user.AddressId).FirstOrDefaultAsync();
            _db.Entry(address).State = EntityState.Deleted;
        }
        _db.Entry(user).State = EntityState.Deleted;
        return await SaveAsync();
    }
    public async Task<List<Categories>> GetAllCategoriesAsync()
    {
        return await _db.Categories.ToListAsync();
    }
    public async Task<bool> AddCategoryAsync(Categories category)
    {
        await _db.Categories.AddAsync(category);
        return await SaveAsync();
    }
    public async Task<bool> DeleteCategoryAsync(Categories category)
    {
        _db.Entry(category).State = EntityState.Deleted;
        return await SaveAsync();
    }
    public async Task<bool> SaveAsync()
    {
        var saved = await _db.SaveChangesAsync();
        return saved > 0 ? true : false;
    }

    public async Task<ApplicationUsers> GetUserByIdAsync(string id)
    {
        return await _db.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<ApplicationUsers>> GetAllAdminsAsync()
    {
        var adminRoleId = await _db.Roles.Where(x => x.Name == Role.Admin).Select(x => x.Id).FirstOrDefaultAsync();

        return await (from role in _db.UserRoles
                      join user in _db.Users
                      on role.UserId equals user.Id
                      where role.RoleId == adminRoleId
                      select user).ToListAsync();
    }

    public async Task<bool> AddPaymentTypeAsync(PaymentType type)
    {
        await _db.PaymentTypes.AddAsync(type);
        return await SaveAsync();
    }

    public async Task<bool> AddShippingTypeAsync(ShippingType type)
    {
        await _db.ShippingTypes.AddAsync(type);
        return await SaveAsync();
    }

    public async Task<List<PaymentType>> GetAllPaymentTypesAsync()
    {
        return await _db.PaymentTypes.ToListAsync();
    }

    public async Task<List<ShippingType>> GetAllShippingTypesAsync()
    {
        return await _db.ShippingTypes.ToListAsync();
    }

    public async Task<bool> DeletePaymentTypeAsync(PaymentType type)
    {
        _db.Entry(type).State = EntityState.Deleted;
        return await SaveAsync();
    }

    public async Task<bool> DeleteShippingTypeAsync(ShippingType type)
    {
        _db.Entry(type).State = EntityState.Deleted;
        return await SaveAsync();
    }
}
