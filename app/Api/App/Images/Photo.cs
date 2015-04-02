using System.Collections.Generic;
using Api.App.Images.Exif;

namespace Api.App.Images
{
    public class Photo
    {
        public string OwnerId { get; set; }
        public ImageData Image { get; set; }
        public Genre Genre { get; set; }
        public ExifData Exif { get; set; }
        public string Title { get; set; }
        public string Story { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}