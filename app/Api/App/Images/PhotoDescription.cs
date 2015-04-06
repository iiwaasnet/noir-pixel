using System.Collections.Generic;
using Api.App.Images.Exif;

namespace Api.App.Images
{
    public class PhotoDescription
    {
        public Genre Genre { get; set; }
        public ExifData Exif { get; set; }
        public string Title { get; set; }
        public string Story { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}