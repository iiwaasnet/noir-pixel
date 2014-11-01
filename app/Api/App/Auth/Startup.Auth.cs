using System;
using Api.Providers;
using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Api.App.Auth
{
    public static class StartupAuth
    {
        public static void ConfigureAuth(this IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(() => DependencyInjection.GetContainer().Resolve<ApplicationIdentityContext>());
            app.CreatePerOwinContext(() => DependencyInjection.GetContainer().Resolve<ApplicationUserManager>());
            app.CreatePerOwinContext(() => DependencyInjection.GetContainer().Resolve<ApplicationRoleManager>());

            var authOptions = DependencyInjection.GetContainer().Resolve<AuthOptions>();

            app.UseOAuthBearerTokens(authOptions.AuthServerOptions);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            app.UseGoogleAuthentication(authOptions.GoogleAuthOptions);
        }
    }
}