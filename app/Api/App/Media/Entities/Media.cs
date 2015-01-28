using Api.App.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Media.Entities
{
    public class Media : Entity
    {
        public const string CollectionName = "media";

        [BsonRepresentation(BsonType.ObjectId)]
        public string OwnerId { get; set; }

        public MediaLocation Location { get; set; }
        public string Uri { get; set; }
    }
}