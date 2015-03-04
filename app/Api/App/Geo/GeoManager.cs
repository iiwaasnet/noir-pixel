using System.Collections.Generic;
using System.Linq;
using Api.App.Db;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Api.App.Geo
{
    public class GeoManager : IGeoManager
    {
        private readonly MongoDatabase db;

        public GeoManager(IAppDbProvider appDbProvider)
        {
            db = appDbProvider.GetDatabase();
        }

        public IEnumerable<Country> GetCountries()
        {
            var countries = db.GetCollection<Entities.Country>(Entities.Country.CollectionName);

            return countries.FindAll().Select(c => new Country
                                                   {
                                                       Code = c.Code,
                                                       Name = c.Name
                                                   });
        }

        public Country GetCountry(string countryCode)
        {
            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                var countries = db.GetCollection<Entities.Country>(Entities.Country.CollectionName);

                var country = countries.FindOne(Query<Entities.Country>.EQ(c => c.Code, countryCode.ToUpper()));

                return (country != null)
                           ? new Country {Code = country.Code, Name = country.Name}
                           : null;
            }

            return null;
        }
    }
}