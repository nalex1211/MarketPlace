using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required!")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [DisplayName("Remember me?")]
    public bool RememberMe { get; set; }
}
