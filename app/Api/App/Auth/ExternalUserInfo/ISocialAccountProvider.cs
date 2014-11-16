using System.Threading.Tasks;

namespace Api.App.Auth.ExternalUserInfo
{
    public interface ISocialAccountProvider
    {
        Task<ExternalUserInfo> GetUserInfo(string userId, string accessToken);

        Task<ParsedExternalAccessToken> VerifyAccessToken(string accessToken);

        string Provider { get; }
    }
}