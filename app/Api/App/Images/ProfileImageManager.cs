using System;
using System.Drawing;
using System.IO;
using Api.App.Db;
using Api.App.Db.Extensions;
using Api.App.Images.Config;
using Api.App.Images.Entities;
using Api.App.Media;
using Api.App.Media.Config;
using JsonConfigurationProvider;
using MongoDB.Driver;

namespace Api.App.Images
{
    public class ProfileImageManager : IProfileImageManager
    {
        private readonly ImagesConfiguration config;
        private readonly MediaConfiguration mediaConfig;
        private readonly IImageProcessor imageProcessor;
        private readonly MongoDatabase db;

        public ProfileImageManager(IAppDbProvider appDbProvider,
                                   IImageProcessor imageProcessor,
                                   IConfigProvider configProvider)
        {
            config = configProvider.GetConfiguration<ImagesConfiguration>();
            mediaConfig = configProvider.GetConfiguration<MediaConfiguration>();
            this.imageProcessor = imageProcessor;
            db = appDbProvider.GetDatabase();
        }

        public ProfileImage SaveImage(string userName, string fileName)
        {
            var profile = db.GetProfile(userName);
            var profileImage = new Entities.ProfileImage();

            var fullViewFile = GenerateFullViewFileName(profile.Id, Path.GetExtension(fileName));
            var thumbnailFile = GenerateThumbnailFileName(profile.Id, Path.GetExtension(fileName));
            var imageInfo = imageProcessor.CreateProfileImage(fileName, fullViewFile, profile.Id);
            profileImage.FullView = new MediaData
                                    {
                                        MediaId = imageInfo.MediaId,
                                        Url = imageInfo.Url
                                    };
            imageInfo = imageProcessor.CreateProfileImageThumbnail(fileName, thumbnailFile, profile.Id);
            profileImage.Thumbnail = new MediaData
                                     {
                                         MediaId = imageInfo.MediaId,
                                         Url = imageInfo.Url
                                     };

            return new ProfileImage
                   {
                       FullViewUrl = profileImage.FullView.Url,
                       ThumbnailUrl = profileImage.Thumbnail.Url
                   };
        }

        private string GenerateThumbnailFileName(string id, string ext)
        {
            return Path.Combine(GenerateProfileImagesFolder(id),
                                string.Format(config.ProfileImages.ThumbnailNameTemplate, id, ext));
        }

        private string GenerateFullViewFileName(string id, string ext)
        {
            return Path.Combine(GenerateProfileImagesFolder(id),
                                string.Format(config.ProfileImages.FullViewNameTemplate, id, ext));
        }

        private string GenerateProfileImagesFolder(string id)
        {
            return Path.Combine(string.Format(mediaConfig.ProfileImagesFolderTemplate, id), id);
        }

        public void DeleteImage(string userName)
        {
            throw new NotImplementedException();
        }

        public Size AvatarSize()
        {
            return new Size(config.ProfileImages.FullView.Width, config.ProfileImages.FullView.Height);
        }

        public Size ThumbnailSize()
        {
            return new Size(config.ProfileImages.Thumbnail.Width, config.ProfileImages.Thumbnail.Height);
        }
    }
}