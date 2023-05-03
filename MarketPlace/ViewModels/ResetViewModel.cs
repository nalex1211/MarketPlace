using System.ComponentModel.DataAnnotations;

namespace MarketPlace.ViewModels;

public class ResetViewModel
{
    [Required(ErrorMessage = "You should enter password!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Confirm password")]
    [Required(ErrorMessage = "Confirmation is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}
