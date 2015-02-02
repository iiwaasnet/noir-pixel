using System.Collections.Generic;

namespace Api.App.Geo
{
    public interface IGeoManager
    {
        Country GetCountry(string countryCode);
        IEnumerable<Country> GetCountries();
    }
}