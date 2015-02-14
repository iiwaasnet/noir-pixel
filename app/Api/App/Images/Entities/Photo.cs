using Api.App.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Images.Entities
{
    public class Photo : Entity
    {
        public const string CollectionName = "photos";

        [BsonRepresentation(BsonType.ObjectId)]
        public string OwnerId { get; set; }

        public PhotoFullViewData FullView { get; set; }
        public MediaData Preview { get; set; }
        public MediaData Thumbnail { get; set; }
    }
}