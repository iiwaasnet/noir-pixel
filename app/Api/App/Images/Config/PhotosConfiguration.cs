namespace Api.App.Images.Config
{
    public class PhotosConfiguration
    {
        public string FullViewNameTemplate { get; set; }
        public string ThumbnailNameTemplate { get; set; }
        public string PreviewNameTemplate { get; set; }
        public SizeConstraints FullView { get; set; }
        public Dimensions Preview { get; set; }
        public Dimensions Thumbnail { get; set; }
    }
}