using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models;

public class Products
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public double Price { get; set; }
    [ForeignKey("Categories")]
    public int? CategoryId { get; set; }
    public Categories? Category { get; set; }
    public List<Orders>? Orders { get; set; }
}
