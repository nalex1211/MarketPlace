using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.Models;

public class Orders
{
    public int Id { get; set; }
    public string CreationDate { get; set; } = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public double TotalPrice { get; set; }
    public int? DeliveryAddressId { get; set; }
    public AddressesForOrders? DeliveryAddress { get; set; }
    public string? UserId { get; set; }
    public ApplicationUsers? User { get; set; }
    public int ShippingTypeId { get; set; }
    public ShippingType? ShippingType { get; set;}
    public int PaymentTypeId { get; set; }
    public PaymentType? PaymentType { get; set; }
    public int ProductCount { get; set; }
    public Status Status { get; set; }
    public List<Products>? Product { get; set; } = new List<Products>();
}
