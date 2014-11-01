namespace Api.App.Auth
{
    public class LocalAccessTokenModel
    {
        public string Provider { get; set; }
        public string ExternalAccessToken { get; set; }
    }
}