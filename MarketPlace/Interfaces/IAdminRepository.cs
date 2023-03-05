using MarketPlace.Models;
using MarketPlace.ViewModels;

namespace MarketPlace.Interfaces;

public interface IAdminRepository
{
    Task<List<ApplicationUsers>> GetAllUsersAsync();
    Task<List<Categories>> GetAllCategoriesAsync();
    Task<ApplicationUsers> GetUserByIdAsync(string id);
    Task<List<ApplicationUsers>> GetAllAdminsAsync();
    bool AddAdminEmail(AdminEmails model);
    bool DeleteUser(ApplicationUsers user);
    bool DeleteCategory(Categories category);
    bool AddCategory(Categories category);
    bool Save();
}
