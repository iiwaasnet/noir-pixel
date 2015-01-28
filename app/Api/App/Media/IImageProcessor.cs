namespace Api.App.Media
{
    public interface IImageProcessor
    {
        ImageInfo CreatePhoto(string source, string destination, string ownerId);
        ImageInfo CreatePhotoPreview(string source, string destination, string ownerId);
        ImageInfo CreatePhotoThumbnail(string source, string destination, string ownerId);

        ImageInfo CreateProfileImage(string source, string destination, string ownerId);
        ImageInfo CreateProfileImageThumbnail(string source, string destination, string ownerId);
    }
}