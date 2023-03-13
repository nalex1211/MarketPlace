using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Models;

public class ApplicationUsers : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? AddressId { get; set; }
    public Addresses? Address { get; set; }
    //public string? Country { get; set; }
    //public string? City { get; set; }
    //public string? Street { get; set; }
    //public string? PostalCode { get; set; }
}
