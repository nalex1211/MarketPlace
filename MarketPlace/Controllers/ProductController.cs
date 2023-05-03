using ClosedXML.Excel;
using MarketPlace.Data;
using MarketPlace.Helpers;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using MarketPlace.ViewModels;
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
        var payments = await _productRepository.GetAllPaymentTypesAsync();
        var shippings = await _productRepository.GetAllShippingTypeTypesAsync();
        ViewData["Payments"] = payments.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        ViewData["Shippings"] = shippings.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        var model = new BuyViewModel()
        {
            Products = new List<Products> { product },
            ApplicationUsers = user
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> BuyProductPost(BuyViewModel model, int[] selectedItemIds, bool paymentMethod)
    {
        var productsToBuy = await _productRepository.GetAllCartProductsAsync(selectedItemIds);
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var deliveryAddress = new AddressesForOrders()
        {
            Country = model.ApplicationUsers.Address.Country,
            City = model.ApplicationUsers.Address.City,
            Street = model.ApplicationUsers.Address.Street,
            PostalCode = model.ApplicationUsers.Address.PostalCode
        };
        await _productRepository.AddDeliveryAddressAsync(deliveryAddress);

        var order = new Orders()
        {
            FirstName = model.ApplicationUsers.FirstName,
            LastName = model.ApplicationUsers.LastName,
            Email = model.ApplicationUsers.Email,
            ShippingTypeId = model.Order.ShippingTypeId,
            PaymentTypeId = model.Order.PaymentTypeId,
            UserId = userId,
            ProductCount = productsToBuy.Count(),
            Status = Status.OnTheWay,
            DeliveryAddress = deliveryAddress,
            TotalPrice = productsToBuy.Sum(x => x.Price)
        };
        await _productRepository.CreateOrderAsync(order);
        foreach (var product in productsToBuy)
        {
            var cartProductToDelete = await _productRepository.FindCartProductsAsync(product.Id, userId);
            if (cartProductToDelete is not null)
            {
                await _productRepository.RemoveFromCartAsync(cartProductToDelete);
            }
            order.Product.Add(product);
        }
        await _productRepository.SaveAsync();
        return RedirectToAction("Index", "Home");
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
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        if (model.CategoryId == 0)
        {
            TempData["CategoryError"] = "You must choose a category!";
            return View(model);
        }
        if (!ModelState.IsValid)
        {
            ViewData["Categories"] = categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            return View(model);
        }
        model.UserId = userId;
        _productRepository.AddAsync(model);
        return RedirectToAction("AllUserProducts");
    }
    public async Task<IActionResult> AllUserProducts()
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var userProducts = await _productRepository.GetAllUserProductsAsync(userId);
        return View(userProducts);
    }
    public async Task<IActionResult> ViewOwnProduct(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        return View(product);
    }
    public async Task<IActionResult> EditOwnProduct(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        var categories = await _productRepository.GetAllCategoriesAsync();
        ViewData["Categories"] = categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        return View(product);
    }
    [HttpPost]
    public async Task<IActionResult> EditOwnProduct(Products model)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _productRepository.GetAllCategoriesAsync();
            ViewData["Categories"] = categories.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
            return View(model);
        }
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        await _productRepository.UpdateAsync(model);
        return RedirectToAction("AllUserProducts");
    }
    [HttpPost]
    public async Task<IActionResult> DeleteOwnProduct(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        await _productRepository.DeleteAsync(product);
        return RedirectToAction("AllUserProducts");
    }
    public IActionResult AddProductExcel()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddProductExcel(IFormFile fileExcel)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        if (fileExcel != null)
        {
            using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
            {
                await fileExcel.CopyToAsync(stream);
                using (var workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                {
                    foreach (var worksheet in workBook.Worksheets)
                    {
                        foreach (var row in worksheet.RowsUsed())
                        {
                            try
                            {
                                var category = await _productRepository.GetCategoryByCellAsync(row.Cell(4).Value.ToString());
                                if (category is null)
                                {
                                    TempData["NoCategory"] = $"No such category \"{row.Cell(4).Value}\"";
                                    return View();
                                }
                                var product = new Products();
                                product.Name = row.Cell(1).Value.ToString();
                                product.Category = category;
                                product.Description = row.Cell(2).Value.ToString();
                                product.Price = double.Parse(row.Cell(3).Value.ToString());
                                product.UserId = userId;
                                await _productRepository.AddAsync(product);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }
                    }
                }
            }
            _productRepository.SaveAsync();
        }
        return RedirectToAction("AllUserProducts", "Product");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Export()
    {
        int rowCount = 1;
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        using (var workBook = new XLWorkbook(XLEventTracking.Disabled))
        {
            var userProducts = await _productRepository.GetAllUserProductsAsync(userId);
            var worksheet = workBook.Worksheets.Add("All products");
            foreach (var product in userProducts)
            {
                worksheet.Cell(rowCount, 1).Value = product.Name;
                worksheet.Cell(rowCount, 2).Value = product.Description;
                worksheet.Cell(rowCount, 3).Value = product.Price;
                worksheet.Cell(rowCount, 4).Value = product.Category.Name;
                rowCount++;
            }
            using (var stream = new MemoryStream())
            {
                workBook.SaveAs(stream);
                await stream.FlushAsync();
                return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName = $"Products_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                };
            }
        }
    }
    public async Task<IActionResult> ViewCart()
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var userCart = await _productRepository.GetCartProductsAsync(userId);
        return View(userCart);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var cart = new Cart()
        {
            ProductId = productId,
            UserId = userId
        };
        await _productRepository.AddToCartAsync(cart);
        return RedirectToAction("ViewCart");
    }
    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int[] selectedItemIds)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        foreach (var cartProductId in selectedItemIds)
        {
            var cartProductToDelete = await _productRepository.FindCartProductsAsync(cartProductId, userId);
            if (cartProductToDelete is not null)
            {
                await _productRepository.RemoveFromCartAsync(cartProductToDelete);
            }
        }
        await _productRepository.SaveAsync();
        return RedirectToAction("ViewCart");
    }
    [HttpPost]
    public async Task<IActionResult> BuyFromCart(int[] selectedItemIds)
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var user = await _userRepository.GetUserByIdAsync(userId);
        var products = await _productRepository.GetAllCartProductsAsync(selectedItemIds);
        var payments = await _productRepository.GetAllPaymentTypesAsync();
        var shippings = await _productRepository.GetAllShippingTypeTypesAsync();
        ViewData["Payments"] = payments.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        ViewData["Shippings"] = shippings.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();
        var model = new BuyViewModel()
        {
            Products = products,
            ApplicationUsers = user
        };
        return View("BuyProduct", model);
    }
}
