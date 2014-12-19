using Api.App.Entities;
using MongoDB.Bson;

namespace Api.App.Users
{
    public class Geo : IEntity
    {
        public ObjectId _id { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}