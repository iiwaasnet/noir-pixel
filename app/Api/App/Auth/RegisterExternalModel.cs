using System.ComponentModel.DataAnnotations;
using Api.App.Errors;

namespace Api.App.Auth
{
    public class RegisterExternalModel
    {
        [Required(ErrorMessage = ApiErrors.Validation.ValueRequired)]
        public string Provider { get; set; }
        [Required(ErrorMessage = ApiErrors.Validation.ValueRequired)]
        public string ExternalAccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }
}