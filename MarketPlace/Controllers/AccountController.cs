using MarketPlace.Helpers;
using MarketPlace.Models;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static MarketPlace.Helpers.AllEmails;
using static MarketPlace.Helpers.Roles;

namespace MarketPlace.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUsers> _userManager;
    private readonly SignInManager<ApplicationUsers> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly EmailService _emailService;

    public AccountController(UserManager<ApplicationUsers> userManager, SignInManager<ApplicationUsers> signInManager,
        RoleManager<IdentityRole> roleManager, EmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailService = emailService;
    }

    public IActionResult Login()
    {
        var response = new LoginViewModel();
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            TempData["Error"] = "User with this email doesn't exist!";
            return View(model);
        }
        var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!passwordCheck)
        {
            TempData["Error"] = "Wrong password!";
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(model);
    }

    public IActionResult Register()
    {
        var response = new RegisterViewModel();
        return View(response);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (!await _roleManager.RoleExistsAsync(Role.Admin))
            await _roleManager.CreateAsync(new IdentityRole(Role.Admin));

        if (!await _roleManager.RoleExistsAsync(Role.User))
            await _roleManager.CreateAsync(new IdentityRole(Role.User));

        if (!await _roleManager.RoleExistsAsync(Role.Vendor))
            await _roleManager.CreateAsync(new IdentityRole(Role.Vendor));

        var emailExists = await _userManager.FindByEmailAsync(model.Email);
        if (emailExists != null)
        {
            TempData["Error"] = "This email already exists!";
            return View(model);
        }

        var usernameExists = await _userManager.FindByNameAsync(model.Username);
        if (usernameExists != null)
        {
            TempData["Error"] = "This username already exists!";
            return View(model);
        }

        if (Emails.adminEmails.Contains(model.Email))
        {
            var adminUser = new ApplicationUsers()
            {
                FirstName = model.Name,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Username,
                PhoneNumber = model.PhoneNumber
            };

            var adminResponse = await _userManager.CreateAsync(adminUser, model.Password);
            if (adminResponse.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(adminUser);
                await _userManager.AddToRoleAsync(adminUser, Role.Admin);
                await _userManager.ConfirmEmailAsync(adminUser, token);
            }
            return RedirectToAction("Index", "Home");
        }

        var newUser = new ApplicationUsers()
        {
            FirstName = model.Name,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Username,
            PhoneNumber = model.PhoneNumber
        };

        var newUserResponse = await _userManager.CreateAsync(newUser, model.Password);
        if (newUserResponse.Succeeded)
        {
            await _userManager.AddToRoleAsync(newUser, Role.User);
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> SendEmail(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return View("Error");
        }
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var url = Url.Action("ConfirmAccount", "Account",
            new { id = id, token = token }, protocol: Request.Scheme);

        await _emailService.SendEmailAsync(user.Email, url);
        return View();
    }
    public async Task<IActionResult> ConfirmAccount(string id, string token)
    {

        if (id == null || token == null)
        {
            return View("Error");
        }

        var user = await _userManager.FindByIdAsync(id);

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return View();
        }

        return View("Error");
    }

}
