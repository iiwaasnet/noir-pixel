using Api.App.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Images.Entities
{
    public class ProfileImage : Entity
    {
        [BsonIgnoreIfNull]
        public ProfileImageData FullView { get; set; }
        public ProfileImageData Thumbnail { get; set; }
    }

    public class ProfileImageData
    {
        public string Url { get; set; }
        [BsonIgnoreIfNull]
        public string LocalFile { get; set; }
    }
}