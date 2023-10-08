using MarketPlace.Data;
using MarketPlace.Helpers;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static MarketPlace.Helpers.Roles;

namespace MarketPlace.Controllers;
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUsers> _userManager;
    private readonly SignInManager<ApplicationUsers> _signInManager;

    public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUsers> userManager, SignInManager<ApplicationUsers> signInManager)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    public async Task<IActionResult> MyProfile()
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _userRepository.GetUserByIdAsync(userId);

        var roles = await _userManager.GetRolesAsync(user);

        var model = new UserViewModel()
        {
            ApplicationUsers = user,
            Role = roles.FirstOrDefault()
        };
        return View(model);
    }
    public async Task<IActionResult> EditProfile()
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _userRepository.GetUserByIdAsync(userId);

        return View(user);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(ApplicationUsers model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _userRepository.GetUserByIdNoTracking(userId);

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Email = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.UserName = model.UserName;

        await _userRepository.UpdateProfileAsync(user);
        return RedirectToAction("MyProfile");
    }
    public async Task<IActionResult> AddAddress()
    {
        var address = new Addresses();
        return View(address);
    }
    [HttpPost]
    public async Task<IActionResult> AddAddress(Addresses model)
    {
        
        await _userRepository.AddAddressAsync(model);

        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _userRepository.GetUserByIdAsync(userId);
        user.Address = model;
        await _userManager.UpdateAsync(user);
        return RedirectToAction("MyProfile");
    }
    public async Task<IActionResult> ChangeAddress()
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _userRepository.GetUserByIdNoTracking(userId);
        var address = user.Address;
        return View(address);
    }
    [HttpPost]
    public async Task<IActionResult> ChangeAddress(Addresses model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _userRepository.GetUserByIdAsync(userId);

        user.Address.Country = model.Country;
        user.Address.City = model.City;
        user.Address.Street = model.Street;
        user.Address.PostalCode = model.PostalCode;
        await _userManager.UpdateAsync(user);
        return RedirectToAction("MyProfile");
    }
    [HttpPost]
    public async Task<IActionResult> BecomeVendor()
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user.Address is null)
        {
            TempData["NoAddress"] = "You must add your address first!";
            return RedirectToAction("AddAddress");
        }

        if (!user.EmailConfirmed)
        {
            TempData["NotConfirmed"] = "You must verify your email to become vendor!";
            return RedirectToAction("MyProfile");
        }
        await _userManager.AddToRoleAsync(user, Role.Vendor);
        await _userManager.RemoveFromRoleAsync(user, Role.User);
        await _userRepository.SaveAsync();
        await _signInManager.RefreshSignInAsync(user);
        return RedirectToAction("MyProfile");
    }
}
