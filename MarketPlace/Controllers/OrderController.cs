using MarketPlace.Data;
using MarketPlace.Helpers;
using MarketPlace.Interfaces;
using MarketPlace.Models;
using MarketPlace.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controllers;
public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrderController(IOrderRepository orderRepository, IHttpContextAccessor httpContextAccessor)
    {
        _orderRepository = orderRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> ViewOrder()
    {
        var userId = _httpContextAccessor.HttpContext.User.GetUserId();
        var userOrders = await _orderRepository.GetUserOrdersAsync(userId);
        return View(userOrders);
    }

    public async Task<IActionResult> GetOrdersFromVendor()
    {
        var vendorId = _httpContextAccessor.HttpContext.User.GetUserId();
        var orders = await _orderRepository.GetOrdersFromVendorAsync(vendorId);
        return View(orders);
    }
    [HttpPost]
    public async Task<IActionResult> EditOrder(Orders model)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        await _orderRepository.EditOrderFromVendorAsync(model);
        return RedirectToAction("GetOrdersFromVendor");
    }
}
