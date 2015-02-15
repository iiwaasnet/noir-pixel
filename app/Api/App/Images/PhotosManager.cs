using System.IO;
using Api.App.Db;
using Api.App.Db.Extensions;
using Api.App.Framework;
using Api.App.Images.Config;
using Api.App.Images.Entities;
using Api.App.Media;
using Api.App.Media.Config;
using Api.App.Media.Extensions;
using Diagnostics;
using JsonConfigurationProvider;
using MongoDB.Driver;

namespace Api.App.Images
{
    public class PhotosManager : IPhotosManager
    {
        private readonly PhotosConfiguration config;
        private readonly MediaConfiguration mediaConfig;
        private readonly IImageProcessor imageProcessor;
        private readonly MongoDatabase db;
        private readonly ILogger logger;
        private readonly IMediaManager mediaManager;

        public PhotosManager(IAppDbProvider appDbProvider,
                             IImageProcessor imageProcessor,
                             IConfigProvider configProvider,
                             IMediaManager mediaManager,
                             ILogger logger)
        {
            this.logger = logger;
            this.mediaManager = mediaManager;
            config = configProvider.GetConfiguration<ImagesConfiguration>().Photos;
            mediaConfig = configProvider.GetConfiguration<MediaConfiguration>();
            this.imageProcessor = imageProcessor;
            db = appDbProvider.GetDatabase();
        }

        public Photo SavePhoto(string userName, string fileName)
        {
            //TODO: Assert photo dimensions
            var profile = db.GetProfile(userName);
            var photo = new Entities.Photo
            {
                OwnerId = profile.Id,
                ShortId = SUIDGenerator.Generate()
            };

            var photoId = photo.Id;

            var fullViewFile = FullViewFileName(profile.Id, photoId, fileName.GetFileExtension());
            var previewFileName = FullPreviewFileName(profile.Id, photoId, fileName.GetFileExtension());
            var thumbnailFile = ThumbnailFileName(profile.Id, photoId, fileName.GetFileExtension());

            EnsureTargetDirectoryExists(profile.Id, photoId);

            var imageInfo = imageProcessor.CreatePhoto(fileName, fullViewFile, profile.Id);
            photo.FullView = new PhotoFullViewData
                             {
                                 MediaId = imageInfo.MediaId,
                                 Uri = imageInfo.Uri,
                                 Height = imageInfo.Height,
                                 Width = imageInfo.Width
                             };
            imageInfo = imageProcessor.CreatePhotoPreview(fileName, previewFileName, profile.Id);
            photo.Preview = new MediaData
                            {
                                MediaId = imageInfo.MediaId,
                                Uri = imageInfo.Uri
                            };
            imageInfo = imageProcessor.CreatePhotoThumbnail(fileName, thumbnailFile, profile.Id);
            photo.Thumbnail = new MediaData
                              {
                                  MediaId = imageInfo.MediaId,
                                  Uri = imageInfo.Uri
                              };

            var collection = db.GetCollection<Entities.Photo>(Entities.Photo.CollectionName);
            collection.Insert(photo).LogCommandResult(logger);

            return new Photo
                   {
                       Id = photo.ShortId,
                       FullViewUrl = photo.FullView.Uri,
                       PreviewUrl = photo.Preview.Uri,
                       ThumbnailUrl = photo.Thumbnail.Uri
                   };
        }

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