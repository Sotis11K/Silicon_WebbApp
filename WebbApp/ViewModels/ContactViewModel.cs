using System.ComponentModel.DataAnnotations;

namespace WebbApp.ViewModels;

public class ContactViewModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name="Full Name", Prompt = "Enter your full name")]
    public string FullName { get; set; } = null!;

    [Required]
    [Display(Name = "Email address", Prompt = "Enter your email address")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Display(Name = "Service (optional)", Prompt = "Choose the service you are interested in")]
    public string? Service { get; set; }

    [Required]
    [Display(Name = "Message", Prompt = "Enter your message here...")]
    public string Message { get; set; } = null!;
}