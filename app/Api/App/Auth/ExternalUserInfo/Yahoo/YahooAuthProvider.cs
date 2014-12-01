using System.Security.Claims;
using System.Threading.Tasks;
using Owin.Security.Providers.Yahoo;

namespace Api.App.Auth.ExternalUserInfo.Yahoo
{
    public class YahooAuthProvider : IYahooAuthenticationProvider
    {
        public Task Authenticated(YahooAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            context.Identity.AddClaim(new Claim("AccessTokenSecret", context.AccessTokenSecret));
            return Task.FromResult<object>(null);
        }

        public Task ReturnEndpoint(YahooReturnEndpointContext context)
        {
            return Task.FromResult<object>(null);
        }

        //public Task Authenticated(JY.Owin.Security.Yahoo.YahooAuthenticatedContext context)
        //{
        //    context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
        //    context.Identity.AddClaim(new Claim("AccessTokenSecret", context.AccessTokenSecret));
        //    return Task.FromResult<object>(null);
        //}

        //public Task ReturnEndpoint(JY.Owin.Security.Yahoo.YahooReturnEndpointContext context)
        //{
        //    return Task.FromResult<object>(null);
        //}

        //public void ApplyRedirect(YahooApplyRedirectContext context)
        //{
        //    context.Response.Redirect(context.RedirectUri);
        //}
    }
}