using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Media.Entities
{
    public class MediaLocation
    {
        public string Location { get; set; }
        public bool Remote { get; set; }
    }
}