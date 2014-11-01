using System.ComponentModel.DataAnnotations;
using Api.Errors;

namespace Api.App.Auth
{
    public class RegisterModel
    {
        [Required(ErrorMessage = ValidationErrors.RequiredField)]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = ValidationErrors.InvalidEmail)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = ValidationErrors.NotSame)]
        public string ConfirmPassword { get; set; }

    }
}