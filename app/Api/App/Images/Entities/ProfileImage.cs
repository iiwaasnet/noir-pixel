using Api.App.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Images.Entities
{
    public class ProfileImage : Entity
    {
        [BsonIgnoreIfNull]
        public MediaData FullView { get; set; }
        public MediaData Thumbnail { get; set; }
    }
}