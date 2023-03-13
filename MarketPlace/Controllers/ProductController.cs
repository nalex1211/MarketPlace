using MarketPlace.Helpers;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarketPlace.Controllers;
public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;

    public ProductController(IProductRepository productRepository, IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository)
    {
        _productRepository = productRepository;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }
    public async Task<IActionResult> ViewProduct(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        return View(product);
    }
    public async Task<IActionResult> BuyProduct(int id)
    {
        if (!User.Identity.IsAuthenticated)
        {
            TempData["NotLoggedIn"] = "You must be logged in!";
            return RedirectToAction("Index", "Home");
        }
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _userRepository.GetUserByIdAsync(userId);
        var product = await _productRepository.GetProductByIdAsync(id);

        var model = new BuyViewModel()
        {
            Products = product,
            ApplicationUsers = user
        };
        return View(model);
    }
    public async Task<IActionResult> AddProduct()
    {
        var categories = await _productRepository.GetAllCategoriesAsync();
        ViewData["Categories"] = categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddProduct(Products model)
    {
        var categories = await _productRepository.GetAllCategoriesAsync();
        if (!ModelState.IsValid)
        {
            ViewData["Categories"] = categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            return View(model);
        }
        _productRepository.Add(model);
        return RedirectToAction("Index", "Home");
    }
}
