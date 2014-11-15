using System;
using Api.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin.Security.Providers.GooglePlus;

namespace Api.App.Auth
{
    public class AuthOptions
    {
        public AuthOptions()
        {
            PublicClientId = "self";
            AuthServerOptions = new OAuthAuthorizationServerOptions
                                {
                                    AllowInsecureHttp = true,
                                    TokenEndpointPath = new PathString("/token"),
                                    AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                                    AuthorizeEndpointPath = new PathString("/account/external-login"),
                                    Provider = new ApplicationOAuthProvider(PublicClientId)
                                };
            GoogleAuthOptions = new GoogleOAuth2AuthenticationOptions
                                {
                                    ClientId = "180426522882-j68nln4atebaf3r1ddb6lgc7h4im2c7j.apps.googleusercontent.com",
                                    ClientSecret = "yg_imJKvQIjYu40L01d23QZ4",
                                    CallbackPath = new PathString("/signin-google"),
                                    Provider = new GoogleAuthProvider()
                                };
            GooglePlusAuthOptions = new GooglePlusAuthenticationOptions
            {
                ClientId = "",
                ClientSecret = "",
                CallbackPath = new PathString(),
                Provider = ,
            };
        }

        public OAuthAuthorizationServerOptions AuthServerOptions { get; private set; }
        public GoogleOAuth2AuthenticationOptions GoogleAuthOptions { get; private set; }
        public GooglePlusAuthenticationOptions GooglePlusAuthOptions { get; private set; }
        public string PublicClientId { get; private set; }
    }
}