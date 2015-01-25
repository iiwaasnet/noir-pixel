using System.Drawing;

namespace Api.App.Images
{
    public interface IProfileImageManager
    {
        string SaveImage(string userName, string fileName);
        void DeleteImage(string userName);
        Size AvatarSize();

        Size ThumbnailSize();
    }
}