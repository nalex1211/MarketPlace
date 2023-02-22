using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUsers> GetUserByIdAsync(string id);
    Task<ApplicationUsers> GetUserByIdNoTracking(string id);
    bool AddAddress(Addresses model);
    bool Save();
}
