using System.ComponentModel.DataAnnotations;

namespace MarketPlace.ViewModels;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "You should enter your email!"), EmailAddress]
    public string Email { get; set; }
}
