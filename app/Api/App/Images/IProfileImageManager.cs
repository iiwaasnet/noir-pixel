using System.Collections.Generic;
using Api.App.Media;

namespace Api.App.Images
{
    public interface IProfileImageManager
    {
        ProfileImage SaveImage(string userName, string fileName);
        void DeleteImage(string userName);
        IEnumerable<MediaConstraint> GetMediaConstraints();
        ProfileImage SaveThumbnailLink(string userName, string url);
    }
}