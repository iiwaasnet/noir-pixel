using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.App.Images
{
    public interface IPhotosManager
    {
        Task<ImageData> SavePhoto(string userName, string fileName);
        Task<PendingPhotos> GetPendingPhotos(string userName, int? offset, int? count);
        Task<Photo> GetPhotoForEdit(string userName, string shortId);
        Task<IEnumerable<Genre>> GetPhotoGenres();
        Task UpdatePhotoDescription(string userName, string shortId, PhotoDescription description);
        Task DeletePhoto(string userName, string shortId);
    }
}