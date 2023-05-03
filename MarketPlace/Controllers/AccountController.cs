using MarketPlace.Data;
using MarketPlace.Helpers;
using MarketPlace.Models;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using static MarketPlace.Helpers.Roles;

namespace MarketPlace.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUsers> _userManager;
    private readonly SignInManager<ApplicationUsers> _signInManager;
    private readonly EmailService _emailService;

    public AccountController(UserManager<ApplicationUsers> userManager, SignInManager<ApplicationUsers> signInManager,
       EmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
        if (user is null)
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

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
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

        var emailExists = await _userManager.FindByEmailAsync(model.Email);
        if (emailExists is not null)
        {
            TempData["Error"] = "This email already exists!";
            return View(model);
        }

        var usernameExists = await _userManager.FindByNameAsync(model.Username);
        if (usernameExists is not null)
        {
            TempData["Error"] = "This username already exists!";
            return View(model);
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

        var descriptions = string.Empty;
        foreach (var item in newUserResponse.Errors)
        {
            descriptions += string.Concat(item.Description, "\n");
        }
        ModelState.AddModelError("ConfirmPassword", descriptions);
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
        if (user is null)
        {
            return View("Error");
        }
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var url = Url.Action("ConfirmAccount", "Account",
            new { id = id, token = token }, protocol: Request.Scheme);

        await _emailService.SendEmailAsync(user.Email, url, "Account confirmation");
        return View();
    }
    public IActionResult ForgotPassword()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            TempData["Error"] = "User with this email doesn't exist!";
            return View(model);
        }
        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            TempData["Error"] = "This email isn't confirmed. Please confirm it!";
            return View(model);
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Action("ResetPassword", "Account",
            new { userId = user.Id, token = token, email = user.Email },
                protocol: Request.Scheme);

        await _emailService.SendEmailAsync(model.Email, callbackUrl, "Password reset");
        return View("PasswordConfirmation");
    }
    public IActionResult ResetPassword(string userId, string token, string email)
    {
        if (userId == null || token == null)
        {
            return View("Error");
        }
        var model = new ResetViewModel
        {
            Email = email,
            Token = token
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            return View(model);
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (!result.Succeeded)
        {
            var descriptions = string.Empty;
            foreach (var item in result.Errors)
            {
                descriptions += string.Concat(item.Description, "\n");
            }
            ModelState.AddModelError("Password", descriptions);
            return View(model);
        }
        return View("ResettedPassword");
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
