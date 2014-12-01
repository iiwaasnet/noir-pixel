using System.ComponentModel.DataAnnotations;

namespace Api.App.Auth
{
    public class RegisterExternalModel
    {
        [Required]
        public string Provider { get; set; }
        [Required]
        public string ExternalAccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }
}