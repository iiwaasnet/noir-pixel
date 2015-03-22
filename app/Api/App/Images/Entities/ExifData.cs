using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Images.Entities
{
    public class ExifData
    {
        [BsonIgnoreIfNull]
        public string Copyright { get; set; }
        [BsonIgnoreIfNull]
        public DateTime? DateTimeTaken { get; set; }
        [BsonIgnoreIfNull]
        public double? ExposureTime { get; set; }
        [BsonIgnoreIfNull]
        public ushort? Iso { get; set; }
        [BsonIgnoreIfNull]
        public double? FStop { get; set; }
        [BsonIgnoreIfNull]
        public double? FocalLength { get; set; }
        [BsonIgnoreIfNull]
        public string CameraModel { get; set; }
        [BsonIgnoreIfNull]
        public string ShutterSpeed { get; set; }
        [BsonIgnoreIfNull]
        public string LensModel { get; set; }
    }
}