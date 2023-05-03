using MarketPlace.Data;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUsers> _userManager;

    public UserRepository(ApplicationDbContext db, UserManager<ApplicationUsers> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    public async Task<ApplicationUsers> GetUserByIdAsync(string id)
    {
        return await _db.Users.Include(user => user.Address).FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<ApplicationUsers> GetUserByIdNoTracking(string id)
    {
        return await _db.Users.Include(user => user.Address).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<bool> SaveAsync()
    {
        var saved = await _db.SaveChangesAsync();
        return saved > 0 ? true : false;
    }
    public async Task<bool> AddAddressAsync(Addresses model)
    {
        await _db.Addresses.AddAsync(model);
        return await SaveAsync();
    }

    public async Task<bool> UpdateProfileAsync(ApplicationUsers model)
    {
        _db.Update(model);
        return await SaveAsync();
    }
}
