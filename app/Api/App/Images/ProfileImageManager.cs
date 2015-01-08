using System;
using System.Drawing;
using Api.App.Images.Config;
using JsonConfigurationProvider;

namespace Api.App.Images
{
    public class ProfileImageManager : IProfileImageManager
    {
        private readonly ImagesConfiguration config;

        public ProfileImageManager(IConfigProvider configProvider)
        {
            config = configProvider.GetConfiguration<ImagesConfiguration>();
        }

        public Size AvatarSize()
        {
            return new Size(config.ProfileImages.Avatar.Width, config.ProfileImages.Avatar.Height);
        }

        public Size ThumbnailSize()
        {
            return new Size(config.ProfileImages.Thumbnail.Width, config.ProfileImages.Thumbnail.Height);
        }

        public Uri DefaultAvatarUri()
        {
            return new Uri("http://default-avatar");
        }
    }
}