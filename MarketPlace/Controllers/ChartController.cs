using MarketPlace.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var categories = _db.Categories.Include(x => x.Products).ToList();
        var lst = new List<object>
        {
            new[] { "Category", "Product count" }
        };
        foreach (var c in categories)
        {
            lst.Add(new object[] { c.Name, c.Products?.Count() });
        }
        return new JsonResult(lst);
    }
}
