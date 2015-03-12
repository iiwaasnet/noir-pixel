using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Owin.Security.Providers.GooglePlus;
using Owin;

namespace Api.App.Auth
{
    public static class StartupAuth
    {
        public static void ConfigureAuth(this IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(() => DependencyInjection.GetContainer().Resolve<ApplicationUserManager>());
            app.CreatePerOwinContext(() => DependencyInjection.GetContainer().Resolve<ApplicationRoleManager>());

            var authOptions = DependencyInjection.GetContainer().Resolve<AuthOptions>();

            app.UseOAuthBearerTokens(authOptions.AuthServerOptions);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

         
            app.UseGooglePlusAuthentication(authOptions.GooglePlusAuthOptions);
            app.UseFacebookAuthentication(authOptions.FacebookAuthOptions);
            app.UseTwitterAuthentication(authOptions.TwitterAuthOptions);
            //app.UseYahooAuthentication(authOptions.YahooAuthOptions);
        }
    }
}