using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Profiles.Entities
{
    public class Geo
    {
        [BsonIgnoreIfNull]
        public string CountryCode { get; set; }

        [BsonIgnoreIfNull]
        public string Country { get; set; }

        [BsonIgnoreIfNull]
        public string City { get; set; }
    }
}