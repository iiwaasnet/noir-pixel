using System.Threading.Tasks;

namespace Api.App.Profiles
{
    public interface IProfilesManager
    {
        Task<UserProfile> GetUserProfile(string userName, bool includePrivateData);
        Task UpdatePublicInfo(string userName, ProfilePublicInfo info);
        Task UpdatePrivateInfo(string userName, ProfilePrivateInfo info);
    }
}