using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Media.Entities
{
    public class MediaLocation
    {
        [BsonIgnoreIfNull]
        public string LocalPath { get; set; }
        [BsonIgnoreIfNull]
        public string Url { get; set; }
    }
}