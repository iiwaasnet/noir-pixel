using System;
using System.Linq;
using System.Threading.Tasks;
using Api.App.Db;
using Api.App.Db.Extensions;
using Api.App.Exceptions;
using Api.App.Framework;
using Api.App.Images.Config;
using Api.App.Images.Entities;
using Api.App.Images.Exif;
using Api.App.Media;
using Api.App.Media.Config;
using Api.App.Media.Extensions;
using Diagnostics;
using MongoDB.Driver;
using TypedConfigProvider;
using ExifData = Api.App.Images.Exif.ExifData;

namespace Api.App.Images
{
    public partial class PhotosManager : IPhotosManager
    {
        private readonly PhotosConfiguration config;
        private readonly MediaConfiguration mediaConfig;
        private readonly IImageProcessor imageProcessor;
        private readonly IMongoDatabase db;
        private readonly ILogger logger;
        private const int DefaultPhotoCount = 20;
        private readonly IImageValidator imageValidator;
        private readonly IExifReader exifReader;

        public PhotosManager(IAppDbProvider appDbProvider,
                             IImageProcessor imageProcessor,
                             IConfigProvider configProvider,
                             IImageValidator imageValidator,
                             IExifReader exifReader,
                             ILogger logger)
        {
            this.logger = logger;
            this.imageValidator = imageValidator;
            config = configProvider.GetConfiguration<ImagesConfiguration>().Photos;
            mediaConfig = configProvider.GetConfiguration<MediaConfiguration>();
            this.imageProcessor = imageProcessor;
            db = appDbProvider.GetDatabase();
            this.exifReader = exifReader;
        }

        public async Task<ImageData> SavePhoto(string userName, string fileName)
        {
            imageValidator.Assert(fileName, GetMediaConstraints());

            var profile = await db.GetProfile(userName);
            var photo = new Entities.Photo
                        {
                            OwnerId = profile.Id,
                            ShortId = SUIDGenerator.Generate(),
                            Status = PhotoStatus.Pending
                        };

            var photoId = photo.InitEntityId();

            var fullViewFile = FullViewFileName(profile.Id, photoId, fileName.GetFileExtension());
            var previewFileName = FullPreviewFileName(profile.Id, photoId, fileName.GetFileExtension());
            var thumbnailFile = ThumbnailFileName(profile.Id, photoId, fileName.GetFileExtension());

            EnsureTargetDirectoryExists(profile.Id, photoId);

            var imageInfo = await imageProcessor.CreatePhoto(fileName, fullViewFile, profile.Id);
            photo.FullView = new PhotoFullViewData
                             {
                                 MediaId = imageInfo.MediaId,
                                 Uri = imageInfo.Uri,
                                 Height = imageInfo.Height,
                                 Width = imageInfo.Width
                             };
            imageInfo = await imageProcessor.CreatePhotoPreview(fileName, previewFileName, profile.Id);
            photo.Preview = new MediaData
                            {
                                MediaId = imageInfo.MediaId,
                                Uri = imageInfo.Uri
                            };
            imageInfo = await imageProcessor.CreatePhotoThumbnail(fileName, thumbnailFile, profile.Id);
            photo.Thumbnail = new MediaData
                              {
                                  MediaId = imageInfo.MediaId,
                                  Uri = imageInfo.Uri
                              };
            photo.Exif = ReadExif(fullViewFile);
            var collection = db.GetCollection<Entities.Photo>(Entities.Photo.CollectionName);
            await collection.InsertOneAsync(photo);

            return new ImageData
                   {
                       Id = photo.ShortId,
                       FullViewUrl = photo.FullView.Uri,
                       PreviewUrl = photo.Preview.Uri,
                       ThumbnailUrl = photo.Thumbnail.Uri
                   };
        }

        public async Task<PendingPhotos> GetPendingPhotos(string userName, int? offset, int? count)
        {
            offset = offset ?? 0;
            count = count ?? DefaultPhotoCount;
            count = Math.Min(count.Value, DefaultPhotoCount);

            var profile = await db.GetProfile(userName);
            var collection = db.GetCollection<Entities.Photo>(Entities.Photo.CollectionName);
            var pendingPhotos = await collection.Find(p => p.OwnerId == profile.Id && p.Status == PhotoStatus.Pending)
                                                .Sort(new SortDefinitionBuilder<Entities.Photo>().Descending(p => p.Id))
                                                .Skip(offset.Value)
                                                .Limit(count.Value)
                                                .ToListAsync();

            return new PendingPhotos
                   {
                       Photos = pendingPhotos.Select(p => new ImageData
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

        public async Task<Photo> GetPhotoForEdit(string userName, string shortId)
        {
            var profile = await db.GetProfile(userName);
            var collection = db.GetCollection<Entities.Photo>(Entities.Photo.CollectionName);
            var photo = await collection.Find(p => p.OwnerId == profile.Id && p.ShortId == shortId)
                                        .FirstOrDefaultAsync();
            AssertPhotoFoundAndNotPublished(shortId, photo);

            return new Photo
                   {
                       Category = photo.Category,
                       Exif = new ExifData
                              {
                                  CameraModel = photo.Exif.CameraModel,
                                  Copyright = photo.Exif.Copyright,
                                  DateTimeTaken = photo.Exif.DateTimeTaken,
                                  ExposureTime = photo.Exif.ExposureTime,
                                  FStop = photo.Exif.FStop,
                                  FocalLength = photo.Exif.FocalLength,
                                  ShutterSpeed = photo.Exif.ShutterSpeed,
                                  Iso = photo.Exif.Iso,
                                  LensModel = photo.Exif.LensModel
                              },
                       OwnerId = photo.OwnerId,
                       Image = new ImageData
                               {
                                   Id = photo.ShortId,
                                   FullViewUrl = photo.FullView.Uri,
                                   PreviewUrl = photo.Preview.Uri,
                                   ThumbnailUrl = photo.Thumbnail.Uri
                               },
                       Story = photo.Story
                   };
        }

        private static void AssertPhotoFoundAndNotPublished(string shortId, Entities.Photo photo)
        {
            if (photo == null)
            {
                throw new NotFoundException(string.Format("Photo {0} is not found!", shortId));
            }
            if (photo.PublishedToGallery)
            {
                throw new InvalidPotoStateException(string.Format("Photo {0} is already published!", shortId));
            }
        }
    }
}