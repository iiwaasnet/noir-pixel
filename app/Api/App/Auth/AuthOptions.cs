using System;
using Api.Providers;
using Microsoft.Owin;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;

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
                                    CallbackPath = new PathString("/signin-google"),
                                    Provider = new GoogleAuthProvider()
                                };
        }

        public OAuthAuthorizationServerOptions AuthServerOptions { get; private set; }
        public GoogleOAuth2AuthenticationOptions GoogleAuthOptions { get; private set; }
        public string PublicClientId { get; private set; }
    }
}