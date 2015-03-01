using System.Collections.Generic;
using Api.App.Media;

namespace Api.App.Images
{
    public interface IPhotosManager
    {
        Photo SavePhoto(string userName, string fileName);
        PendingPhotos GetPendingPhotos(string userName, int? offset, int? count);
        IEnumerable<MediaConstraint> GetMediaConstraints();
    }
}