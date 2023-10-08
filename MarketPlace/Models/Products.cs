using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models;

public class Products
{
    public int Id { get; set; }
    [Required(ErrorMessage ="Product name is required!")]
    [StringLength(50, MinimumLength = 5)]
    public string Name { get; set; }
    [Required(ErrorMessage ="Product description is required!")]
    [StringLength(100, MinimumLength = 10)]
    public string Description { get; set; }
    [Required(ErrorMessage ="Product price is required!")]
    public double Price { get; set; }
    public int CategoryId { get; set; }
    public Categories? Category { get; set; }
    public string? UserId { get; set; }
    public ApplicationUsers? User { get; set; }
    public List<Orders>? Orders { get; set; } = new List<Orders>();
}
