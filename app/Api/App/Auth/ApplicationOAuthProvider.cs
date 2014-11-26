using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.App.Auth.Config;
using Api.Models;
using JsonConfigurationProvider;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Shared;

namespace Api.App.Auth
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string publicClientId;
        private readonly string allowedOrigin;
        private readonly TimeSpan tokenExpirationTime;

        public ApplicationOAuthProvider(IConfigProvider configProvider)
        {
            // TODO: Add HTTPS support, if needed
            allowedOrigin = string.Format("http://{0}", configProvider.GetConfiguration<SiteConfiguration>().SiteUrl);
            var authConfiguration = configProvider.GetConfiguration<AuthConfiguration>();
            publicClientId = authConfiguration.PublicClientId;
            tokenExpirationTime = authConfiguration.AccessTokenExpirationTime;

            AssertPublicClientIdNotNull();
        }

        private void AssertPublicClientIdNotNull()
        {
            if (string.IsNullOrWhiteSpace(publicClientId))
            {
                throw new ArgumentNullException("publicClientId");
            }
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            EnableCors(context);

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
            var cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);

            var properties = CreateAuthenticationProperties(user);
            var ticket = new AuthenticationTicket(oAuthIdentity, properties);
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesIdentity);
        }

        private AuthenticationProperties CreateAuthenticationProperties(ApplicationUser user)
        {
            return new AuthenticationProperties(new Dictionary<string, string>
                                                {
                                                    {"userName", user.UserName}
                                                })
                   {
                       IssuedUtc = DateTime.UtcNow,
                       ExpiresUtc = DateTime.UtcNow.Add(tokenExpirationTime)
                   };
        }

        private void EnableCors(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedClientOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? allowedOrigin;

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] {allowedClientOrigin});
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId == publicClientId)
            {
                if (context.RedirectUri.StartsWith(allowedOrigin))
                {
                    context.Validated();
                }
            }

            return Task.FromResult<object>(null);
        }
    }
}