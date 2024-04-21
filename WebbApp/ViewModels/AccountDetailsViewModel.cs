using System.ComponentModel.DataAnnotations;

namespace WebbApp.ViewModels
{


    public class AccountDetailsViewModel
    {


        public AccountBasicInfo? BasicInfo { get; set; } = new AccountBasicInfo();
        public AccountAddressInfo? AddressInfo { get; set; }
    }
    public class AccountBasicInfo
    {
        [DataType(DataType.ImageUrl)]
        public string? ProfileImage { get; set; }

        [Display(Name = "First Name", Prompt = "Enter your first name", Order = 0)]
        [Required(ErrorMessage = "Invalid first name")]
        [MinLength(2, ErrorMessage = "Invalid first name")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name", Prompt = "Enter your last name", Order = 1)]
        [Required(ErrorMessage = "Invalid last name")]
        [MinLength(2, ErrorMessage = "Invalid last name")]

        public string LastName { get; set; } = null!;

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address", Prompt = "Enter your email address", Order = 2)]
        [RegularExpression("^[^@\\s]+@[^@\\s]+\\.[^@\\s]{2,}$", ErrorMessage = "Invalid email address")]

        public string Email { get; set; } = null!;

        [Display(Name = "Phone", Prompt = "Enter your phone", Order = 3)]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone is required")]

        public string Phone { get; set; } = null!;

        [Display(Name = "Bio", Prompt = "Add a short bio...", Order = 4)]
        [DataType(DataType.MultilineText)]

        public string? Bio { get; set; }
    }

    public class AccountAddressInfo
    {
        [Display(Name = "Address", Prompt = "Enter your address line", Order = 0)]
        [Required(ErrorMessage = "Invalid first name")]
        public string Addressline_1 { get; set; } = null;

        [Display(Name = "Address 2", Prompt = "Enter your second address line", Order = 1)]

        public string? Addressline_2 { get; set; }


        [Display(Name = "Postal code", Prompt = "Enter your postal code", Order = 2)]
        [Required(ErrorMessage = "Postal code is required")]
        [DataType(DataType.PostalCode)]

        public string PostalCode { get; set; } = null;

        [Display(Name = "City", Prompt = "Enter your City", Order = 3)]
        [Required(ErrorMessage = "City is required ")]


        public string City { get; set; } = null;
    }
}
