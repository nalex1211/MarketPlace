using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models;

public class AddressesForOrders
{
    [Key]
    public int Id { get; set; }
    [StringLength(30, MinimumLength = 5)]
    public string? Country { get; set; }
    [StringLength(30, MinimumLength = 5)]
    public string? City { get; set; }
    [StringLength(50, MinimumLength = 10)]
    public string? Street { get; set; }
    [StringLength(20, MinimumLength = 3)]
    public string? PostalCode { get; set; }
}
