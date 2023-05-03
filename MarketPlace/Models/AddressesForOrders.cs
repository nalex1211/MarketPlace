using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models;

public class AddressesForOrders
{
    [Key]
    public int Id { get; set; }
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
}
