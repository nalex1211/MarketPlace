using MarketPlace.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MarketPlace.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ChartController : ControllerBase
{
    private readonly ApplicationDbContext _db;

    public ChartController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet("JsonData")]
    public JsonResult JsonData()
    {
        var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var orders = _db.Orders.Include(order => order.Product)
                 .Where(order => order.Product.Any(product => product.UserId == vendorId)).ToList();
        var products = _db.Products.ToList();

        var productCounts = from order in orders
                            from product in order.Product
                            join p in products on product.Id equals p.Id
                            group p by p.Name into productGroup
                            select new
                            {
                                ProductName = productGroup.Key,
                                Count = productGroup.Count()
                            };

        var lst = new List<object>
    {
        new[] { "Order", "Product count" }
    };

        foreach (var ratio in productCounts)
        {
            lst.Add(new object[] { ratio.ProductName, ratio.Count });
        }

        return new JsonResult(lst);
    }


    [HttpGet("JsonData1")]
    public JsonResult JsonData1()
    {
        var confirmedCount = _db.Users.Where(u => u.EmailConfirmed).ToList().Count();
        var notConfirmedCount = _db.Users.Where(u => !u.EmailConfirmed).ToList().Count();
        var lst = new List<object>()
        {
        new[] { "Status", "Ratio" },
        new object[] { "Confirmed", confirmedCount },
        new object[] { "Not Confirmed", notConfirmedCount }
        };
        return new JsonResult(lst);
    }

}
