namespace Api.App.Images
{
    public interface IProfileImageManager
    {
        ProfileImage SaveImage(string userName, string fileName);
        void DeleteImage(string userName);
        ProfileImage SaveThumbnailLink(string userName, string url);
    }
}