using System.Collections.Generic;
using System.Threading.Tasks;
using Api.App.Db;
using MongoDB.Driver;

namespace Api.App.Geo
{
    public class GeoManager : IGeoManager
    {
        private readonly IMongoDatabase db;

        public GeoManager(IAppDbProvider appDbProvider)
        {
            db = appDbProvider.GetDatabase();
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            var countries = db.GetCollection<Entities.Country>(Entities.Country.CollectionName);

            return await countries
                             .Find(c => true)
                             .Project(c => new Country
                                           {
                                               Code = c.Code,
                                               Name = c.Name
                                           }).ToListAsync();
        }

        public async Task<Country> GetCountry(string countryCode)
        {
            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                var countries = db.GetCollection<Entities.Country>(Entities.Country.CollectionName);

                var country = await countries.Find(c => c.Code == countryCode.ToUpper()).FirstOrDefaultAsync();

                return (country != null)
                           ? new Country {Code = country.Code, Name = country.Name}
                           : null;
            }

            return null;
        }
    }
}