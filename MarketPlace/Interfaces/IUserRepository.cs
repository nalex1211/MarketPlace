using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;

namespace MarketPlace.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUsers> GetUserByIdAsync(string id);
    Task<ApplicationUsers> GetUserByIdNoTracking(string id);
    Task<bool> AddAddressAsync(Addresses model);
    Task<bool> UpdateProfileAsync(ApplicationUsers model);
    Task<bool> SaveAsync();
}
