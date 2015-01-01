using Api.App.Entities;

namespace Api.App.Users.Entities
{
    public class Geo : Entity
    {
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}