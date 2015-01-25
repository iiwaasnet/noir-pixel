using System;
using System.Drawing;
using Api.App.Db;
using Api.App.Images.Config;
using Api.App.Profiles.Entities;
using JsonConfigurationProvider;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Api.App.Images
{
    public class ProfileImageManager : IProfileImageManager
    {
        private readonly ImagesConfiguration config;
        private readonly MongoDatabase db;

        public ProfileImageManager(IAppDbProvider appDbProvider, IConfigProvider configProvider)
        {
            config = configProvider.GetConfiguration<ImagesConfiguration>();
            db = appDbProvider.GetDatabase();
        }

        public string SaveImage(string userName, string fileName)
        {
            var profiles = db.GetCollection<Profile>(Profile.CollectionName);
            var profile = profiles.FindOne(Query.EQ("UserName", userName));

            throw new Exception();
        }

        public void DeleteImage(string userName)
        {
            throw new System.NotImplementedException();
        }

        public Size AvatarSize()
        {
            return new Size(config.ProfileImages.Avatar.Width, config.ProfileImages.Avatar.Height);
        }

        public Size ThumbnailSize()
        {
            return new Size(config.ProfileImages.Thumbnail.Width, config.ProfileImages.Thumbnail.Height);
        }
    }
}