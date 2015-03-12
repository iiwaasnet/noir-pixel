using System.Drawing;
using System.Threading.Tasks;
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

        public async Task<ImageInfo> CreateProfileImage(string source, string destination, string ownerId)
        {
            return await CropSquareAndSaveImage(source, destination, ownerId, config.ProfileImages.FullViewSize);
        }

        public async Task<ImageInfo> CreateProfileImageThumbnail(string source, string destination, string ownerId)
        {
            return await CropSquareAndSaveImage(source, destination, ownerId, config.ProfileImages.ThumbnailSize);
        }

        public async Task<ImageInfo> CreatePhoto(string source, string destination, string ownerId)
        {
            using (var image = new Bitmap(source))
            {
                ImageUtils.SaveJpeg(destination, image);

                var mediaInfo = await mediaManager.SaveMediaFile(destination, ownerId);
                return new ImageInfo
                       {
                           MediaId = mediaInfo.MediaId,
                           Uri = mediaInfo.Uri,
                           Height = image.Height,
                           Width = image.Width
                       };
            }
        }

        public async Task<ImageInfo> CreatePhotoPreview(string source, string destination, string ownerId)
        {
            return await CropSquareAndSaveImage(source, destination, ownerId, config.Photos.PreviewSize);
        }

        public async Task<ImageInfo> CreatePhotoThumbnail(string source, string destination, string ownerId)
        {
            return await CropSquareAndSaveImage(source, destination, ownerId, config.Photos.ThumbnailSize);
        }

        private async Task<ImageInfo> CropSquareAndSaveImage(string source, string destination, string ownerId, int size)
        {
            using (var image = new Bitmap(source))
            {
                ImageUtils.SaveJpeg(destination,
                                    ImageUtils.CropImage(ImageUtils.ResizeImageForCrop(image, size), size, size));

                var mediaInfo = await mediaManager.SaveMediaFile(destination, ownerId);
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