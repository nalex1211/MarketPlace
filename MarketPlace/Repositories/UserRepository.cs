using MarketPlace.Data;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    public async Task<ApplicationUsers> GetUserByIdAsync(string id)
    {
        return await _db.Users.Include(user => user.Address).FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task<ApplicationUsers> GetUserByIdNoTracking(string id)
    {
        return await _db.Users.Include(user => user.Address).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }
    public bool Save()
    {
        var saved = _db.SaveChanges();
        return saved > 0 ? true : false;
    }
    public bool AddAddress(Addresses model)
    {
        _db.Addresses.Add(model);
        return Save();
    }
}
