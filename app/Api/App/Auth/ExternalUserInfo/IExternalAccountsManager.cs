using System.Threading.Tasks;

namespace Api.App.Auth.ExternalUserInfo
{
    public interface IExternalAccountsManager
    {
        //TODO: Introduce parameter object
        Task<ExternalUserInfo> GetUserInfo(string provider, string userId, string accessToken, string accessTokenSecret);

        Task<ParsedExternalAccessToken> VerfiyAccessToken(string provider, string accessToken, string accessTokenSecret);
    }
}