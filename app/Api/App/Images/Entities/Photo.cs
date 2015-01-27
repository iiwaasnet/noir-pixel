using Api.App.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Images.Entities
{
    public class Photo : Entity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string OwnerId { get; set; }
        public PhotoFullViewData FullView { get; set; }
        public PhotoImageData Preview { get; set; }
        public PhotoImageData Thumbnail { get; set; }
    }

    public class PhotoImageData
    {
        public string Url { get; set; }
        [BsonIgnoreIfNull]
        public string LocalFile { get; set; }
    }

    public class PhotoFullViewData : PhotoImageData
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string ShortId { get; set; }
    }
}