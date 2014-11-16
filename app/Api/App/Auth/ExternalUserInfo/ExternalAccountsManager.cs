using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.App.Auth.ExternalUserInfo
{
    public class ExternalAccountsManager : IExternalAccountsManager
    {
        private readonly IDictionary<string, ISocialAccountProvider> socialAccountProviders;

        public ExternalAccountsManager(IEnumerable<ISocialAccountProvider> socialAccountProviders)
        {
            this.socialAccountProviders = CreateSocialAccountProviders(socialAccountProviders);
        }

        public Task<ExternalUserInfo> GetUserInfo(string provider, string userId, string accessToken)
        {
            if (socialAccountProviders.ContainsKey(provider))
            {
                return socialAccountProviders[provider].GetUserInfo(userId, accessToken);
            }

            throw new KeyNotFoundException(provider);
        }

        public Task<ParsedExternalAccessToken> VerfiyAccessToken(string provider, string accessToken)
        {
            if (socialAccountProviders.ContainsKey(provider))
            {
                return socialAccountProviders[provider].VerifyAccessToken(accessToken);
            }

            throw new KeyNotFoundException(provider);
        }

        private static IDictionary<string, ISocialAccountProvider> CreateSocialAccountProviders(IEnumerable<ISocialAccountProvider> socialAccountProviders)
        {
            return socialAccountProviders.ToDictionary(p => p.Provider, p => p);
        }
    }
}