using System.Threading.Tasks;

namespace Api.App.Auth.ExternalUserInfo
{
    public interface IExternalAccountsManager
    {
        Task<ExternalUserInfo> GetUserInfo(string provider, string userId, string accessToken);

        Task<ParsedExternalAccessToken> VerfiyAccessToken(string provider, string accessToken);
    }
}