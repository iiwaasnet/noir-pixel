using System.Collections.Generic;
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

        public string ShortId { get; set; }
        public PhotoStatus Status { get; set; }

        [BsonIgnoreIfNull]
        public Genre Genre { get; set; }

        public bool PublishedToGallery { get; set; }
        public PhotoFullViewData FullView { get; set; }
        public MediaData Preview { get; set; }
        public MediaData Thumbnail { get; set; }

        [BsonIgnoreIfNull]
        public ExifData Exif { get; set; }

        [BsonIgnoreIfNull]
        public string Title { get; set; }

        [BsonIgnoreIfNull]
        public string Story { get; set; }

        [BsonIgnoreIfNull]
        public IEnumerable<Tag> Tags { get; set; }
    }
}