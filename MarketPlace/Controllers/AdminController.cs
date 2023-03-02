using MarketPlace.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controllers;
public class AdminController : Controller
{
    private readonly IAdminRepository _adminRepository;

    public AdminController(IAdminRepository adminRepository)
    {
       _adminRepository = adminRepository;
    }
    public async Task<IActionResult> ViewAllUsers()
    {
        var users = await _adminRepository.GetAllUsersAsync();
        return View(users);
    }
}
