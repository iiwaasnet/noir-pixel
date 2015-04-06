namespace Api.App.Images
{
    public class Photo
    {
        public string OwnerId { get; set; }
        public ImageData Image { get; set; }
        public PhotoDescription Description { get; set; }
    }
}