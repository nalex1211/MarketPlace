using MarketPlace.Helpers;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static MarketPlace.Helpers.AllEmails;

namespace MarketPlace.Controllers;
public class AdminController : Controller
{
    private readonly IAdminRepository _adminRepository;
    private readonly UserManager<ApplicationUsers> userManager;

    public AdminController(IAdminRepository adminRepository, UserManager<ApplicationUsers> userManager)
    {
        _adminRepository = adminRepository;
        this.userManager = userManager;
    }
    public async Task<IActionResult> ViewAllUsers()
    {
        var users = await _adminRepository.GetAllUsersAsync();
        return View(users);
    }
    public IActionResult AddCategory()
    {
        return View();
    }
    [HttpPost]
    public IActionResult AddCategory(Categories category)
    {
        _adminRepository.AddCategory(category);
        return RedirectToAction("MyProfile", "User");
    }
    public async Task<IActionResult> ViewAllCategories()
    {
        var categories = await _adminRepository.GetAllCategoriesAsync();
        return View(categories);
    }
    [HttpPost]
    public IActionResult DeleteCategory(Categories category)
    {
        _adminRepository.DeleteCategory(category);
        return RedirectToAction("MyProfile", "User");
    }
    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _adminRepository.GetUserByIdAsync(id);
        _adminRepository.DeleteUser(user);
        return RedirectToAction("MyProfile", "User");
    }
    public IActionResult AddEmail()
    {
        var model = new AdminEmails();
        return View(model);
    }
    [HttpPost]
    public IActionResult AddEmail(AdminEmails model)
    {
        _adminRepository.AddAdminEmail(model);
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> ViewAllAdminUsers()
    {
        var admins =await _adminRepository.GetAllAdminsAsync();
        return View(admins);
    }
}
