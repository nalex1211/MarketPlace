using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Models;

public class ApplicationUsers : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool AddressAdded { get; set; }
    [ForeignKey("Addresses")]
    public int? AddressId { get; set; }
    public Addresses? Address { get; set; }
}
