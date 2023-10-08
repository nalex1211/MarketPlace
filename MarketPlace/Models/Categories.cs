using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models;

public class Categories
{
    public int Id { get; set; }
    [Required(ErrorMessage ="Name of category is required!")]
    [StringLength(70, MinimumLength = 5)]
    public string Name { get; set; }
    public List<Products>? Products { get; set; } = new List<Products>();
}
