using System.ComponentModel.DataAnnotations;
using WebbApp.Filters;

namespace WebbApp.ViewModels;

public class SignUpViewModel
{


    [Required]
    [Display(Name = "First name", Prompt = "Enter your first name", Order = 0)]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Last name", Prompt = "Enter your last name", Order = 1)]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;

    [Required]
    [Display(Name = "Email", Prompt = "Enter your email address", Order = 2)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Password", Prompt = "********", Order = 3)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [Display(Name = "Confirm Password", Prompt = "********", Order = 4)]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;

    [CheckboxRequired]
    [Display(Name = "I agree to the Terms & Conditions", Order = 5)]
    public bool TermsAndConditions {  get; set; }
}

