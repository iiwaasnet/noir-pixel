using System.Threading.Tasks;

namespace Api.App.Auth.ExternalUserInfo
{
    public interface ISocialAccountProvider
    {
        Task<ExternalUserInfo> GetUserInfo(string userId, string accessToken, string accessTokenSecret);

        Task<ParsedExternalAccessToken> VerifyAccessToken(string accessToken, string accessTokenSecret);

        string Provider { get; }
    }
}