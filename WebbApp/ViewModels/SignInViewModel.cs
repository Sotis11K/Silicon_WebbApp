using System.ComponentModel.DataAnnotations;

namespace WebbApp.ViewModels;

public class SignInViewModel
{
    [Required]
    [Display(Name = "Email", Prompt = "Enter your email address", Order = 0)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Password", Prompt = "********", Order = 1)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;


    [Display(Name = "Remember me", Order = 2)]
    public bool IsPresistent { get; set; }
}

