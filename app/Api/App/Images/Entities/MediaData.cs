using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Images.Entities
{
    public class MediaData
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string MediaId { get; set; }
        public string Url { get; set; } 
    }
}