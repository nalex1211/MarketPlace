using MarketPlace.Data;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using MarketPlace.ViewModels;
using Microsoft.EntityFrameworkCore;

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
    public bool DeleteUser(ApplicationUsers user)
    {
        if (user.Address is not null)
        {
            var address = _db.Addresses.Where(x => x.Id == user.AddressId).FirstOrDefault();
            _db.Addresses.Remove(address);
        }
        _db.Users.Remove(user);
        return Save();
    }
    public async Task<List<Categories>> GetAllCategoriesAsync()
    {
        return await _db.Categories.ToListAsync();
    }
    public bool AddCategory(Categories category)
    {
        _db.Categories.Add(category);
        return Save();
    }
    public bool DeleteCategory(Categories category)
    {
        _db.Categories.Remove(category);
        return Save();
    }
    public bool Save()
    {
        var saved = _db.SaveChanges();
        return saved > 0 ? true : false;
    }

    public async Task<ApplicationUsers> GetUserByIdAsync(string id)
    {
        return await _db.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);
    }

    public bool AddAdminEmail(AdminEmails model)
    {
        _db.AdminEmails.Add(model);
        return Save();
    }

    public async Task<List<ApplicationUsers>> GetAllAdminsAsync()
    {
        return await (from adminEmail in _db.AdminEmails
                      join user in _db.Users
                      on adminEmail.AdminEmail equals user.Email
                      select user).ToListAsync();

    }
}
