using System;
using System.Drawing;
using Api.App.Images;
using Api.App.Images.Config;
using JsonConfigurationProvider;

namespace Api.App.Media
{
    public class ImageProcessor : IImageProcessor
    {
        private readonly IMediaManager mediaManager;
        private readonly ImagesConfiguration config;

        public ImageProcessor(IMediaManager mediaManager, IConfigProvider configProvider)
        {
            this.mediaManager = mediaManager;
            config = configProvider.GetConfiguration<ImagesConfiguration>();
        }

        public ImageInfo CreateProfileImage(string source, string destination, string ownerId)
        {
            using (var image = new Bitmap(source))
            {
                ImageUtils.SaveJpeg(destination,
                                    ImageUtils.CropImage(ImageUtils.ResizeImageForCrop(image, config.ProfileImages.ThumbnailSize),
                                                         config.ProfileImages.ThumbnailSize,
                                                         config.ProfileImages.ThumbnailSize));

                var mediaInfo = mediaManager.SaveMedia(destination, ownerId);
                return new ImageInfo
                       {
                           MediaId = mediaInfo.MediaId,
                           Uri = mediaInfo.Uri,
                           Height = config.ProfileImages.FullViewSize,
                           Width = config.ProfileImages.FullViewSize
                       };
            }
        }

        public ImageInfo CreateProfileImageThumbnail(string source, string destination, string ownerId)
        {
            using (var image = new Bitmap(source))
            {
                ImageUtils.SaveJpeg(destination,
                                    ImageUtils.CropImage(ImageUtils.ResizeImageForCrop(image, config.ProfileImages.ThumbnailSize),
                                                         config.ProfileImages.ThumbnailSize,
                                                         config.ProfileImages.ThumbnailSize));

                var mediaInfo = mediaManager.SaveMedia(destination, ownerId);
                return new ImageInfo
                       {
                           MediaId = mediaInfo.MediaId,
                           Uri = mediaInfo.Uri,
                           Height = config.ProfileImages.ThumbnailSize,
                           Width = config.ProfileImages.ThumbnailSize
                       };
            }
        }

        public ImageInfo CreatePhoto(string source, string destination, string ownerId)
        {
            throw new NotImplementedException();
        }

        public ImageInfo CreatePhotoPreview(string source, string destination, string ownerId)
        {
            throw new NotImplementedException();
        }

        public ImageInfo CreatePhotoThumbnail(string source, string destination, string ownerId)
        {
            throw new NotImplementedException();
        }
    }
}