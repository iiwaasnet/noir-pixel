using System.Threading.Tasks;

namespace Api.App.Users
{
    public interface IUsersManager
    {
        Task<UserHome> GetUserHome(string userName);
        UserProfile GetUserProfile(string userName, bool includePrivateData);
    }
}