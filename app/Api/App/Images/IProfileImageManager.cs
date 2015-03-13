using System.Threading.Tasks;

namespace Api.App.Images
{
    public interface IProfileImageManager
    {
        Task<ProfileImage> SaveImage(string userName, string fileName);
        Task DeleteImage(string userName);
        Task<ProfileImage> SaveThumbnailLink(string userName, string url);
    }
}