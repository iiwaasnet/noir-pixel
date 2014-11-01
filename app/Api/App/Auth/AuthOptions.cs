using System;
using Api.Providers;
using Microsoft.Owin;
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
        }

        public OAuthAuthorizationServerOptions AuthServerOptions { get; private set; }
        public string PublicClientId { get; private set; }
    }
}