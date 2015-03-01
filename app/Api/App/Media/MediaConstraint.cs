namespace Api.App.Media
{
    public class MediaConstraint
    {
        public int MaxFileSizeMB { get; set; }
        public MediaType MediaType { get; set; }
    }

    public enum MediaType
    {
        Jpeg,
        Gif
    }
}