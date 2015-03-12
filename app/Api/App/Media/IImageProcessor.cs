using System.Threading.Tasks;

namespace Api.App.Media
{
    public interface IImageProcessor
    {
        Task<ImageInfo> CreatePhoto(string source, string destination, string ownerId);
        Task<ImageInfo> CreatePhotoPreview(string source, string destination, string ownerId);
        Task<ImageInfo> CreatePhotoThumbnail(string source, string destination, string ownerId);
        Task<ImageInfo> CreateProfileImage(string source, string destination, string ownerId);
        Task<ImageInfo> CreateProfileImageThumbnail(string source, string destination, string ownerId);
    }
}