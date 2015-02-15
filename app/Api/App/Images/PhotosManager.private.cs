using System.IO;

namespace Api.App.Images
{
    public partial class PhotosManager
    {
        private void EnsureTargetDirectoryExists(string userId, string photoId)
        {
            var folder = PhotosFolderName(userId, photoId);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        private string FullPreviewFileName(string userId, string photoId, string ext)
        {
            return Path.Combine(PhotosFolderName(userId, photoId),
                                string.Format(config.PreviewNameTemplate, photoId, ext));
        }

        private string ThumbnailFileName(string userId, string photoId, string ext)
        {
            return Path.Combine(PhotosFolderName(userId, photoId),
                                string.Format(config.ThumbnailNameTemplate, photoId, ext));
        }

        private string FullViewFileName(string userId, string photoId, string ext)
        {
            return Path.Combine(PhotosFolderName(userId, photoId),
                                string.Format(config.FullViewNameTemplate, photoId, ext));
        }

        private string PhotosFolderName(string userId, string photoId)
        {
            return string.Format(mediaConfig.PhotosFolderTemplate, userId, photoId);
        }
    }
}