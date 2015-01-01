using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Users.Entities
{
    public class PostTargetReference
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId TargetId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public PostTargetType TargetType { get; set; }
    }

    public enum PostTargetType
    {
        Photo,
        Wall
    }
}