using System.Threading.Tasks;

namespace Api.App.Images
{
    public interface IProfileImageManager
    {
        Task<ProfileImage> SaveImage(string userName, string fileName);
        void DeleteImage(string userName);
        Task<ProfileImage> SaveThumbnailLink(string userName, string url);
    }
}