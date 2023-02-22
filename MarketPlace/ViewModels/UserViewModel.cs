using MarketPlace.Models;
using Microsoft.AspNetCore.Identity;

namespace MarketPlace.ViewModels;

public class UserViewModel
{
    public ApplicationUsers ApplicationUsers { get; set; }
    public string Role { get; set; }
}
