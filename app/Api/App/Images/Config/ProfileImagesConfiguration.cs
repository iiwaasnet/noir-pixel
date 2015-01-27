namespace Api.App.Images.Config
{
    public class ProfileImagesConfiguration
    {
        public Dimensions FullView { get; set; }
        public Dimensions Thumbnail { get; set; }
        public string FullViewNameTemplate { get; set; }
        public string ThumbnailNameTemplate { get; set; }
    }
}