using Api.App.Auth.Config;
using Api.App.Auth.ExternalUserInfo.Facebook;
using Api.App.Auth.ExternalUserInfo.GPlus;
using Api.App.Auth.ExternalUserInfo.Twitter;
using JsonConfigurationProvider;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security.Twitter;
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
            Twitter = new TwitterAuthenticationOptions
                      {
                          ConsumerKey = authConfig.Twitter.ConsumerKey,
                          ConsumerSecret = authConfig.Twitter.ConsumerSecret,
                          Provider = new TwitterAuthProvider()
                      };
        }

        public OAuthAuthorizationServerOptions AuthServerOptions { get; private set; }
        public GooglePlusAuthenticationOptions GooglePlusAuthOptions { get; private set; }
        public FacebookAuthenticationOptions FacebookAuthOptions { get; private set; }
        public TwitterAuthenticationOptions Twitter { get; private set; }

        public string PublicClientId { get; private set; }
    }
}