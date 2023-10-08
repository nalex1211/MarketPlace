using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarketPlace.Models;

public class ApplicationUsers : IdentityUser
{
    [Required(ErrorMessage ="You must enter your first name!")]
    [StringLength(20)]
    public string FirstName { get; set; }
    [Required(ErrorMessage ="You must enter your last name!")]
    [StringLength(20)]
    public string LastName { get; set; }
    public int? AddressId { get; set; }
    public Addresses? Address { get; set; }
}
