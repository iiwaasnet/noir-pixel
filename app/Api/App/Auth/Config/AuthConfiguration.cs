using System;

namespace Api.App.Auth.Config
{
    public class AuthConfiguration
    {
        public string PublicClientId { get; set; }
        public TimeSpan AccessTokenExpirationTime { get; set; }
        public GooglePlusConfiguration GooglePlus { get; set; }
        public FacebookConfiguration Facebook { get; set; }
        public TwitterConfiguration Twitter { get; set; }
    }
}