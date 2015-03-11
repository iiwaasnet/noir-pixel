using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.App.Geo
{
    public interface IGeoManager
    {
        Task<Country> GetCountry(string countryCode);
        Task<IEnumerable<Country>> GetCountries();
    }
}