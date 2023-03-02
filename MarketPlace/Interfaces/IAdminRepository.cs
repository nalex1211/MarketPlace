using MarketPlace.Models;

namespace MarketPlace.Interfaces;

public interface IAdminRepository
{
    Task<List<ApplicationUsers>> GetAllUsersAsync();
}
