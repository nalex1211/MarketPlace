using MarketPlace.Interfaces;
using MarketPlace.Models;
using MarketPlace.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MarketPlace.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepository;

    public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
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
