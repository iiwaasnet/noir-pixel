namespace Api.App.Images
{
    public interface IProfileImageManager
    {
        ProfileImage SaveImageFile(string userName, string fileName);
        ProfileImage SaveThumbnailLink(string userName, string url);
        void DeleteImage(string userName);
        int ThumbnailSize();
    }
}