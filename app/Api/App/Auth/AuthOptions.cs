using System;
using System.Linq;
using Api.App.Auth.Config;
using Api.App.Auth.ExternalUserInfo.Facebook;
using Api.App.Auth.ExternalUserInfo.GPlus;
using JsonConfigurationProvider;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.OAuth;
using Owin.Security.Providers.GooglePlus;
using WebGrease.Css.Extensions;

namespace Api.App.Auth
{
    public class AuthOptions
    {
        public AuthOptions(IConfigProvider configProvider)
        {
            var authConfig = configProvider.GetConfiguration<AuthConfiguration>();

            PublicClientId = authConfig.PublicClientId;
            AuthServerOptions = new OAuthAuthorizationServerOptions
                                {
                                    AllowInsecureHttp = true,
                                    TokenEndpointPath = new PathString("/token"),
                                    AccessTokenExpireTimeSpan = authConfig.AccessTokenExpirationTime,
                                    AuthorizeEndpointPath = new PathString("/account/external-login"),
                                    Provider = new ApplicationOAuthProvider(configProvider)
                                };
            GooglePlusAuthOptions = new GooglePlusAuthenticationOptions
                                    {
                                        ClientId = authConfig.GooglePlus.ClientId,
                                        ClientSecret = authConfig.GooglePlus.ClientSecret,
                                        Provider = new GooglePlusAuthProvider()
                                    };
            FacebookAuthOptions = new FacebookAuthenticationOptions
                                  {
                                      AppId = authConfig.Facebook.AppId,
                                      AppSecret = authConfig.Facebook.AppSecret,
                                      Provider = new FacebookAuthProvider()
                                  };
            authConfig.Facebook.UserScope.ForEach(FacebookAuthOptions.Scope.Add);
        }

        public OAuthAuthorizationServerOptions AuthServerOptions { get; private set; }
        public GooglePlusAuthenticationOptions GooglePlusAuthOptions { get; private set; }
        public FacebookAuthenticationOptions FacebookAuthOptions { get; private set; }
        public string PublicClientId { get; private set; }
    }
}