using Api.App.Entities;

namespace Api.App.Geo.Entities
{
    public class Country: Entity
    {
        public const string CollectionName = "countries";

        public string Code { get; set; }
        public string Name { get; set; }
    }
}