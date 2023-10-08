using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models;

public class ShippingType
{
    public int Id { get; set; }
    [Required(ErrorMessage ="Shipping type name is required!")]
    [StringLength(70, MinimumLength = 5)]
    public string Name { get; set; }
}
