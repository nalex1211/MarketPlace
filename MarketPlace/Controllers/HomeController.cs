using MarketPlace.Data;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MarketPlace.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ApplicationDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _productRepository = productRepository;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllProdsAndCategsAsync();
        var categories = await _productRepository.GetAllCategoriesAsync();
        var model = new ProductCategoriesVM()
        {
            Products = products,
            Categories = categories
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> FindProduct(string productName)
    {
        if (productName == null)
        {
            TempData["EmptyInput"] = "Please enter product name in input field!";
            return RedirectToAction("Index", "Home");
        }

        var product = await _productRepository.FindProductByNameAsync(productName);
        if (product is null)
        {
            TempData["NoProduct"] = "No such product!";
            return RedirectToAction("Index", "Home");
        }
        var model = new ProductCategoriesVM()
        {
            Products = new List<Products> { product },
            Categories = new List<Categories> { product.Category}
        };
        return View("Index", model);
    }

    [HttpGet]
    public async Task<IActionResult> GetProductNames(string term)
    {
        var products = await _productRepository.GetProductsByNameAsync(term);
        return Json(products.Select(p => new { name = p.Name }));
    }


    public async Task<IActionResult> DisplayCategoryProducts(string category)
    {
        var products = await _productRepository.GetCategoryProductsAsync(category);
        var categories = await _productRepository.GetAllCategoriesAsync();
        var model = new ProductCategoriesVM()
        {
            Products = products,
            Categories = categories
        };
        return View("Index", model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
