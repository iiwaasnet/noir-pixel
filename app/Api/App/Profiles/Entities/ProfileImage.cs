using Api.App.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Profiles.Entities
{
    public class ProfileImage : Entity
    {
        [BsonRepresentation(BsonType.String)]
        public ProfileImageType ImageType { get; set; }
        public string Url { get; set; }
    }

    public enum ProfileImageType
    {
        Avatar,
        Thumbnail
    }
}