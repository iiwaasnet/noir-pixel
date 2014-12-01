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
    }
}