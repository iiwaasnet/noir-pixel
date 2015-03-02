using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
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
using MongoDB.Driver.Builders;

namespace Api.App.Images
{
    public partial class PhotosManager : IPhotosManager
    {
        private readonly PhotosConfiguration config;
        private readonly MediaConfiguration mediaConfig;
        private readonly IImageProcessor imageProcessor;
        private readonly MongoDatabase db;
        private readonly ILogger logger;
        private const int DefaultPhotoCount = 20;
        private readonly IImageValidator imageValidator;

        public PhotosManager(IAppDbProvider appDbProvider,
                             IImageProcessor imageProcessor,
                             IConfigProvider configProvider,
                             IImageValidator imageValidator,
                             ILogger logger)
        {
            this.logger = logger;
            this.imageValidator = imageValidator;
            config = configProvider.GetConfiguration<ImagesConfiguration>().Photos;
            mediaConfig = configProvider.GetConfiguration<MediaConfiguration>();
            this.imageProcessor = imageProcessor;
            db = appDbProvider.GetDatabase();
        }

        public Photo SavePhoto(string userName, string fileName)
        {
            imageValidator.Assert(fileName, GetMediaConstraints());

            var profile = db.GetProfile(userName);
            var photo = new Entities.Photo
                        {
                            OwnerId = profile.Id,
                            ShortId = SUIDGenerator.Generate(),
                            Status = PhotoStatus.Pending
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

        public PendingPhotos GetPendingPhotos(string userName, int? offset, int? count)
        {
            offset = offset ?? 0;
            count = count ?? DefaultPhotoCount;
            count = Math.Min(count.Value, DefaultPhotoCount);

            var profile = db.GetProfile(userName);
            var collection = db.GetCollection<Entities.Photo>(Entities.Photo.CollectionName);
            var pendingPhotos = collection.FindAs<Entities.Photo>(Query.And(Query<Entities.Photo>.EQ(p => p.OwnerId, profile.Id),
                                                                            Query<Entities.Photo>.EQ(p => p.Status, PhotoStatus.Pending)))
                                          .SetSortOrder(SortBy<Entities.Photo>.Descending(p => p.Id))
                                          .SetSkip(offset.Value)
                                          .SetLimit(count.Value);

            return new PendingPhotos
                   {
                       Photos = pendingPhotos.Select(p => new Photo
                                                          {
                                                              Id = p.ShortId,
                                                              FullViewUrl = p.FullView.Uri,
                                                              PreviewUrl = p.Preview.Uri,
                                                              ThumbnailUrl = p.Thumbnail.Uri
                                                          }),
                       Paging = new Paging
                                {
                                    Offset = offset.Value,
                                    Count = count.Value
                                }
                   };
        }

        private IEnumerable<ImageConstraint> GetMediaConstraints()
        {
            yield return new ImageConstraint
                         {
                             ImageFormat = ImageFormat.Jpeg,
                             MaxFileSizeMB = config.MaxFileSizeMB,
                             Size = new SizeConstraints
                                    {
                                        MaxHeight = config.FullViewSize.MaxHeight,
                                        MinHeight = config.FullViewSize.MinHeight,
                                        MaxWidth = config.FullViewSize.MaxWidth,
                                        MinWidth = config.FullViewSize.MinWidth
                                    }
                         };
        }
    }
}