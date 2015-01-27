using System;
using System.Drawing;
using Api.App.Db;
using Api.App.Db.Extensions;
using Api.App.Images.Config;
using Api.App.Images.Entities;
using Api.App.Media;
using Api.App.Media.Config;
using Api.App.Profiles.Entities;
using JsonConfigurationProvider;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

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

        public string SaveImage(string userName, string fileName)
        {
            var profile = db.GetProfile(userName);
            var profileImage = new ProfileImage();

            var destination = GenerateFullViewFileName(profile.Id);
            var imageInfo = imageProcessor.CreateProfileImage(fileName, destination);
        }

        private string GenerateFullViewFileName(string id)
        {
            throw new NotImplementedException();
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