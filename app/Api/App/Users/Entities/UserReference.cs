using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Users.Entities
{
    public class UserReference
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}