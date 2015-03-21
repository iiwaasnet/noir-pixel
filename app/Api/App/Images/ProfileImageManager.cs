using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Api.App.Db;
using Api.App.Db.Extensions;
using Api.App.Images.Config;
using Api.App.Images.Entities;
using Api.App.Media;
using Api.App.Media.Config;
using Api.App.Media.Extensions;
using Api.App.Profiles.Entities;
using Diagnostics;
using TypedConfigProvider;
using MongoDB.Driver;

namespace Api.App.Images
{
    public class ProfileImageManager : IProfileImageManager
    {
        private readonly ProfileImagesConfiguration config;
        private readonly MediaConfiguration mediaConfig;
        private readonly IImageProcessor imageProcessor;
        private readonly IMongoDatabase db;
        private readonly ILogger logger;
        private readonly IMediaManager mediaManager;
        private readonly IImageValidator imageValidator;

        public ProfileImageManager(IAppDbProvider appDbProvider,
                                   IImageProcessor imageProcessor,
                                   IConfigProvider configProvider,
                                   IMediaManager mediaManager,
                                   IImageValidator imageValidator,
                                   ILogger logger)
        {
            this.logger = logger;
            this.mediaManager = mediaManager;
            config = configProvider.GetConfiguration<ImagesConfiguration>().ProfileImages;
            mediaConfig = configProvider.GetConfiguration<MediaConfiguration>();
            this.imageProcessor = imageProcessor;
            this.imageValidator = imageValidator;
            db = appDbProvider.GetDatabase();
        }

        public async Task<ProfileImage> SaveImage(string userName, string fileName)
        {
            imageValidator.Assert(fileName, GetMediaConstraints());

            var profile = await db.GetProfile(userName);
            var currentProfileImage = profile.UserImage;
            var profileImage = new Entities.ProfileImage();

            var profileImageId = profileImage.InitEntityId();

            var fullViewFile = FullViewFileName(profile.Id, profileImageId, fileName.GetFileExtension());
            var thumbnailFile = ThumbnailFileName(profile.Id, profileImageId, fileName.GetFileExtension());

            EnsureTargetDirectoryExists(profile.Id);

            var imageInfo = await imageProcessor.CreateProfileImage(fileName, fullViewFile, profile.Id);
            profileImage.FullView = new MediaData
                                    {
                                        MediaId = imageInfo.MediaId,
                                        Uri = imageInfo.Uri
                                    };
            imageInfo = await imageProcessor.CreateProfileImageThumbnail(fileName, thumbnailFile, profile.Id);
            profileImage.Thumbnail = new MediaData
                                     {
                                         MediaId = imageInfo.MediaId,
                                         Uri = imageInfo.Uri
                                     };

            var collection = db.GetCollection<Profile>(Profile.CollectionName);
            await collection.FindOneAndUpdateAsync(p => p.Id == profile.Id,
                                                   new UpdateDefinitionBuilder<Profile>().Set(p => p.UserImage, profileImage));

            await DeleteProfileImages(currentProfileImage);

            return new ProfileImage
                   {
                       FullViewUrl = profileImage.FullView.Uri,
                       ThumbnailUrl = profileImage.Thumbnail.Uri
                   };
        }

        public async Task<ProfileImage> SaveThumbnailLink(string userName, string url)
        {
            var profile = await db.GetProfile(userName);
            var profileImage = new Entities.ProfileImage();

            var mediaInfo = await mediaManager.SaveMediaUrl(url, profile.Id);
            profileImage.Thumbnail = new MediaData
                                     {
                                         MediaId = mediaInfo.MediaId,
                                         Uri = mediaInfo.Uri
                                     };

            var collection = db.GetCollection<Profile>(Profile.CollectionName);
            await collection.FindOneAndUpdateAsync(p => p.Id == profile.Id,
                                                   new UpdateDefinitionBuilder<Profile>().Set(p => p.UserImage, profileImage));

            return new ProfileImage
                   {
                       ThumbnailUrl = profileImage.Thumbnail.Uri
                   };
        }

        private void EnsureTargetDirectoryExists(string userId)
        {
            var folder = ProfileImagesFolderName(userId);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        private async Task DeleteProfileImages(Entities.ProfileImage profileImage)
        {
            try
            {
                if (profileImage != null)
                {
                    await DeleteMedia(profileImage.FullView);
                    await DeleteMedia(profileImage.Thumbnail);
                }
            }
            catch (Exception err)
            {
                logger.Error("Error deleting profile image: {0}", err);
            }
        }

        private async Task DeleteMedia(MediaData mediaData)
        {
            if (mediaData != null)
            {
                await mediaManager.DeleteMedia(mediaData.MediaId);
            }
        }

        private string ThumbnailFileName(string id, string fileName, string ext)
        {
            return Path.Combine(ProfileImagesFolderName(id),
                                string.Format(config.ThumbnailNameTemplate, fileName, ext));
        }

        private string FullViewFileName(string id, string fileName, string ext)
        {
            return Path.Combine(ProfileImagesFolderName(id),
                                string.Format(config.FullViewNameTemplate, fileName, ext));
        }

        private string ProfileImagesFolderName(string id)
        {
            return string.Format(mediaConfig.ProfileImageFolderTemplate, id);
        }

        public async Task DeleteImage(string userName)
        {
            var profile = await db.GetProfile(userName);
            await DeleteProfileImages(profile.UserImage);

            var collection = db.GetCollection<Profile>(Profile.CollectionName);

            await collection.FindOneAndUpdateAsync(p => p.Id == profile.Id,
                                                   new UpdateDefinitionBuilder<Profile>().Unset(p => p.UserImage));
        }

        private IEnumerable<ImageConstraint> GetMediaConstraints()
        {
            yield return new ImageConstraint
                         {
                             ImageFormat = ImageFormat.Jpeg,
                             MaxFileSizeMB = config.MaxFileSizeMB,
                             Size = new SizeConstraints
                                    {
                                        MaxHeight = Int32.MaxValue,
                                        MinHeight = config.FullViewSize,
                                        MaxWidth = Int32.MaxValue,
                                        MinWidth = config.FullViewSize
                                    }
                         };
        }
    }
}