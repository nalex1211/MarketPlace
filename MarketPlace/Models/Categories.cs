using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models;

public class Categories
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Products> Products { get; set; }
}
