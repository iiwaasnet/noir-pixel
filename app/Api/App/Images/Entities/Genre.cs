using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Images.Entities
{
    public class Genre
    {
        public const string CollectionName = "photoGenres";
        public BsonObjectId Id { get; set; }

        [BsonElement("code")]
        public int Code { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}