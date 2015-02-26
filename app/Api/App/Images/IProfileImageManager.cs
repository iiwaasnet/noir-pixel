namespace Api.App.Images
{
    public interface IProfileImageManager
    {
        ProfileImage SaveImage(string userName, string fileName);
        void DeleteImage(string userName);
        void AssertFileSize(int fileSizeBytes);
        ProfileImage SaveThumbnailLink(string userName, string url);
    }
}