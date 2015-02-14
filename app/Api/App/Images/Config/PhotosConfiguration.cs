namespace Api.App.Images.Config
{
    public class PhotosConfiguration
    {
		public int MaxFileSizeMB { get; set; }
        public string FullViewNameTemplate { get; set; }
        public string ThumbnailNameTemplate { get; set; }
        public string PreviewNameTemplate { get; set; }
        public SizeConstraints FullViewSize { get; set; }
        public int PreviewSize { get; set; }
        public int ThumbnailSize     { get; set; }
    }
}