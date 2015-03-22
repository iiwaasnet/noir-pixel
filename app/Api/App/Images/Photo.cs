using Api.App.Images.Exif;

namespace Api.App.Images
{
    public class Photo
    {
        public string OwnerId { get; set; }
        public ImageData Image { get; set; }
        public int? Category { get; set; }
        public ExifData Exif { get; set; }
        public string Story { get; set; }
    }
}