using System.Collections.Generic;
using System.Threading.Tasks;
using Api.App.Profiles.Entities;

namespace Api.App.Profiles
{
    public interface IProfilesManager
    {
        Task<UserProfile> GetUserProfile(string userName, bool includePrivateData);
        IEnumerable<Country> GetCountries();
    }
}