using System.Threading.Tasks;

namespace Api.App.Profiles
{
    public interface IProfilesManager
    {
        Task<UserProfile> GetUserProfile(string userName, bool includePrivateData);
    }
}