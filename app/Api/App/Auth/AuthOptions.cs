using System;
using Api.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
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
            GooglePlusAuthOptions = new GooglePlusAuthenticationOptions
                                    {
                                        ClientId = "180426522882-j68nln4atebaf3r1ddb6lgc7h4im2c7j.apps.googleusercontent.com",
                                        ClientSecret = "yg_imJKvQIjYu40L01d23QZ4",
                                        Provider = new GooglePlusAuthProvider()
                                    };
            FacebookAuthOptions = new FacebookAuthenticationOptions
                                  {
                                      AppId = "1513484548903273",
                                      AppSecret = "a8b0b56b824359a0ff6e885d7fd3475a",
                                      Provider = new FacebookAuthProvider()
                                  };
        }

        public OAuthAuthorizationServerOptions AuthServerOptions { get; private set; }
        public GooglePlusAuthenticationOptions GooglePlusAuthOptions { get; private set; }
        public FacebookAuthenticationOptions FacebookAuthOptions { get; private set; }
        public string PublicClientId { get; private set; }
    }
}