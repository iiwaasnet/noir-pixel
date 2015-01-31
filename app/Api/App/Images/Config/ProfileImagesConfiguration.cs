namespace Api.App.Images.Config
{
    public class ProfileImagesConfiguration
    {
		public int MaxFileSizeMB { get; set; }
        public int FullViewSize { get; set; }
        public int ThumbnailSize { get; set; }
        public string FullViewNameTemplate { get; set; }
        public string ThumbnailNameTemplate { get; set; }
    }
}