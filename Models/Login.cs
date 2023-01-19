using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Email is required")]
        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DisplayName("Password")]
        public string? Password { get; set; }

        [DisplayName("Remember Me")]
        public bool remember { get; set; }
    }
    public class UserLoginWithId
    {
        [Required(ErrorMessage = "Email is required")]
        [DisplayName("Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DisplayName("Password")]
        public string? Password { get; set; }


        [DisplayName("Remember Me")]
        public bool remember { get; set; }
    }
    public class ChangeMyPasswordModel
    {
        [Display(Name = "Old Password")]
        [Required(ErrorMessage = "Old password required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [Display(Name = "New Password")]
        //[RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password does not match")]
        public string ConfirmPassword { get; set; }

    }
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "New password required", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password does not match")]
        public string ConfirmPassword { get; set; }

        public int userId { get; set; }

    }

}