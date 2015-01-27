namespace Api.App.Media
{
    public interface IImageProcessor
    {
        ImageInfo CreatePhoto(string source, string destination);
        ImageInfo CreatePhotoPreview(string source, string destination);
        ImageInfo CreatePhotoThumbnail(string source, string destination);

        ImageInfo CreateProfileImage(string source, string destination);
        ImageInfo CreateProfileImageThumbnail(string source, string destination);
    }
}