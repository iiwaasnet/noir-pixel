namespace Api.App.Images.Config
{
    public class PhotosConfiguration
    {
        public string RootFolder { get; set; }
        public SizeConstraints FullView { get; set; }
        public Dimensions Preview { get; set; }
        public Dimensions Thumbnail { get; set; }
    }
}