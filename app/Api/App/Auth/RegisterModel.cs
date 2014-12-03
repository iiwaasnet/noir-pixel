using System.ComponentModel.DataAnnotations;
using Api.App.Errors;

namespace Api.App.Auth
{
    public class RegisterModel
    {
        [Required(ErrorMessage = ApiErrors.Validation.RequiredValue)]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = ApiErrors.Validation.InvalidEmail)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = ApiErrors.Validation.NotSame)]
        public string ConfirmPassword { get; set; }

    }
}