﻿using System;
using Api.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Api
{
    public partial class Startup
    {
        public static string PublicClientId { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationIdentityContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            //TODO: decode on clientid
            PublicClientId = "self";
            var oAuthOptions = new OAuthAuthorizationServerOptions
                               {
                                   AllowInsecureHttp = true,
                                   TokenEndpointPath = new PathString("/token"),
                                   AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                                   AuthorizeEndpointPath = new PathString("/api/account/external-login"),
                                   Provider = new ApplicationOAuthProvider(PublicClientId)
                               };

            app.UseOAuthBearerTokens(oAuthOptions);

            //TODO: Walked through until here
            // Investigate why the next lines of code needed

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            //app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}