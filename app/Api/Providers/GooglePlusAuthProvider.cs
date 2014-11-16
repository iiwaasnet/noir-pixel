using System.Security.Claims;
using System.Threading.Tasks;
using Owin.Security.Providers.GooglePlus.Provider;

namespace Api.Providers
{
    public class GooglePlusAuthProvider : IGooglePlusAuthenticationProvider
    {
        public Task Authenticated(GooglePlusAuthenticatedContext context)
        {
            context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
            return Task.FromResult<object>(null);
        }

        public Task ReturnEndpoint(GooglePlusReturnEndpointContext context)
        {
            return Task.FromResult<object>(null);
        }
    }
}