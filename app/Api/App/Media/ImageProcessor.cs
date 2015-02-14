using System;
using System.Drawing;
using System.Security.Policy;
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
            return CropSquareAndSaveImage(source, destination, ownerId, config.ProfileImages.FullViewSize);
        }

        public ImageInfo CreateProfileImageThumbnail(string source, string destination, string ownerId)
        {
            return CropSquareAndSaveImage(source, destination, ownerId, config.ProfileImages.ThumbnailSize);
        }

        public ImageInfo CreatePhoto(string source, string destination, string ownerId)
        {
            using (var image = new Bitmap(source))
            {
                ImageUtils.SaveJpeg(destination, image);

                var mediaInfo = mediaManager.SaveMediaFile(destination, ownerId);
                return new ImageInfo
                {
                    MediaId = mediaInfo.MediaId,
                    Uri = mediaInfo.Uri,
                    Height = image.Height,
                    Width = image.Width
                };
            }
        }

        public ImageInfo CreatePhotoPreview(string source, string destination, string ownerId)
        {
            return CropSquareAndSaveImage(source, destination, ownerId, config.Photos.PreviewSize);
        }

        public ImageInfo CreatePhotoThumbnail(string source, string destination, string ownerId)
        {
            return CropSquareAndSaveImage(source, destination, ownerId, config.Photos.ThumbnailSize);
        }

        private ImageInfo CropSquareAndSaveImage(string source, string destination, string ownerId, int size)
        {
            using (var image = new Bitmap(source))
            {
                ImageUtils.SaveJpeg(destination,
                                    ImageUtils.CropImage(ImageUtils.ResizeImageForCrop(image, size), size, size));

                var mediaInfo = mediaManager.SaveMediaFile(destination, ownerId);
                return new ImageInfo
                {
                    MediaId = mediaInfo.MediaId,
                    Uri = mediaInfo.Uri,
                    Height = size,
                    Width = size
                };
            }
        }
    }
}