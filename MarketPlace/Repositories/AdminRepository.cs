using MarketPlace.Data;
using MarketPlace.Interfaces;
using MarketPlace.Models;
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
}
