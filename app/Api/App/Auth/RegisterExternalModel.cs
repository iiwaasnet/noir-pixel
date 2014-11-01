namespace Api.App.Auth
{
    public class RegisterExternalModel
    {
        public string Provider { get; set; }
        public string ExternalAccessToken { get; set; }
    }
}