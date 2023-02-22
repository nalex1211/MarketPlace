using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models;

public class Categories
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}
