using System.IO;
using Api.App.Images.Entities;

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

        private ExifData ReadExif(string fileName)
        {
            var data = exifReader.ReadExifData(fileName);
            if (data != null)
            {
                return new ExifData
                       {
                           Iso = data.Iso,
                           ShutterSpeed = data.ShutterSpeed,
                           CameraModel = data.CameraModel,
                           Copyright = data.Copyright,
                           DateTimeTaken = data.DateTimeTaken,
                           ExposureTime = data.ExposureTime,
                           FStop = data.FStop,
                           FocalLength = data.FocalLength
                       };
            }

            return null;
        }
    }
}