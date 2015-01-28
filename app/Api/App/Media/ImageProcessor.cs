using System;

namespace Api.App.Media
{
    public class ImageProcessor : IImageProcessor
    {
        private readonly IMediaManager mediaManager;

        public ImageProcessor(IMediaManager mediaManager)
        {
            this.mediaManager = mediaManager;
        }

        public ImageInfo CreateProfileImage(string source, string destination, string ownerId)
        {
            //TODO: Generate new image file
            mediaManager.SaveMedia(destination, ownerId);
            throw new Exception();
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

        public ImageInfo CreateProfileImageThumbnail(string source, string destination, string ownerId)
        {
            throw new NotImplementedException();
        }
    }
}