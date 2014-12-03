using System.ComponentModel.DataAnnotations;
using Api.App.Errors;

namespace Api.App.Auth
{
    public class RegisterExternalModel
    {
        [Required(ErrorMessage = ApiErrors.Validation.RequiredValue)]
        public string Provider { get; set; }
        //[Required(ErrorMessage = ApiErrors.Validation.RequiredValue)]
        [Api.Validation.MinLengthAttribute(100, ErrorMessageResourceName = ApiErrors.Validation.RequiredValue)]
        public string ExternalAccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }
}