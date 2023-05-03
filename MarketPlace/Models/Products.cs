using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models;

public class Products
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public double Price { get; set; }
    public int CategoryId { get; set; }
    public Categories? Category { get; set; }
    public string? UserId { get; set; }
    public ApplicationUsers? User { get; set; }
    public List<Orders>? Orders { get; set; } = new List<Orders>();
}
