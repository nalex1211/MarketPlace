using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models;

public class PaymentType
{
    public int Id { get; set; }
    [Required(ErrorMessage ="Name of payment type is required!")]
    [StringLength(50, MinimumLength = 5)]
    public string Name { get; set; }
    [Required(ErrorMessage ="Description is required!")]
    [StringLength(100, MinimumLength = 20)]
    public string Description { get; set; }
}
