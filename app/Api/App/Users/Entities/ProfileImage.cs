using Api.App.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Users.Entities
{
    public class ProfileImage : Entity
    {
        [BsonRepresentation(BsonType.String)]
        public ProfileImageType ImageType { get; set; }
        public bool UserDefined { get; set; }
        public string Url { get; set; }
    }

    public enum ProfileImageType
    {
        Avatar,
        Thumbnail
    }
}