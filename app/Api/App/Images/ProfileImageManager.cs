using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Api.App.Db;
using Api.App.Db.Extensions;
using Api.App.Exceptions;
using Api.App.Images.Config;
using Api.App.Images.Entities;
using Api.App.Media;
using Api.App.Media.Config;
using Api.App.Media.Extensions;
using Api.App.Profiles.Entities;
using Diagnostics;
using JsonConfigurationProvider;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Api.App.Images
{
    public class ProfileImageManager : IProfileImageManager
    {
        private readonly ProfileImagesConfiguration config;
        private readonly MediaConfiguration mediaConfig;
        private readonly IImageProcessor imageProcessor;
        private readonly MongoDatabase db;
        private readonly ILogger logger;
        private readonly IMediaManager mediaManager;

        public ProfileImageManager(IAppDbProvider appDbProvider,
                                   IImageProcessor imageProcessor,
                                   IConfigProvider configProvider,
                                   IMediaManager mediaManager,
                                   ILogger logger)
        {
            this.logger = logger;
            this.mediaManager = mediaManager;
            config = configProvider.GetConfiguration<ImagesConfiguration>().ProfileImages;
            mediaConfig = configProvider.GetConfiguration<MediaConfiguration>();
            this.imageProcessor = imageProcessor;
            db = appDbProvider.GetDatabase();
        }

        public ProfileImage SaveImage(string userName, string fileName)
        {
            var profile = db.GetProfile(userName);
            var currentProfileImage = profile.UserImage;
            var profileImage = new Entities.ProfileImage();

            var profileImageId = profileImage.Id;

            var fullViewFile = FullViewFileName(profile.Id, profileImageId, fileName.GetFileExtension());
            var thumbnailFile = ThumbnailFileName(profile.Id, profileImageId, fileName.GetFileExtension());

            EnsureTargetDirectoryExists(profile.Id);

            var imageInfo = imageProcessor.CreateProfileImage(fileName, fullViewFile, profile.Id);
            profileImage.FullView = new MediaData
                                    {
                                        MediaId = imageInfo.MediaId,
                                        Uri = imageInfo.Uri
                                    };
            imageInfo = imageProcessor.CreateProfileImageThumbnail(fileName, thumbnailFile, profile.Id);
            profileImage.Thumbnail = new MediaData
                                     {
                                         MediaId = imageInfo.MediaId,
                                         Uri = imageInfo.Uri
                                     };

            var collection = db.GetCollection<Profile>(Profile.CollectionName);
            collection.Update(Query<Profile>.EQ(p => p.Id, profile.Id),
                              Update<Profile>.Set(p => p.UserImage, profileImage))
                      .LogCommandResult(logger);

            DeleteProfileImages(currentProfileImage);

            return new ProfileImage
                   {
                       FullViewUrl = profileImage.FullView.Uri,
                       ThumbnailUrl = profileImage.Thumbnail.Uri
                   };
        }

        public ProfileImage SaveThumbnailLink(string userName, string url)
        {
            var profile = db.GetProfile(userName);
            var profileImage = new Entities.ProfileImage();

            var mediaInfo = mediaManager.SaveMediaUrl(url, profile.Id);
            profileImage.Thumbnail = new MediaData
                                     {
                                         MediaId = mediaInfo.MediaId,
                                         Uri = mediaInfo.Uri
                                     };

            var collection = db.GetCollection<Profile>(Profile.CollectionName);
            collection.Update(Query<Profile>.EQ(p => p.Id, profile.Id),
                              Update<Profile>.Set(p => p.UserImage, profileImage))
                      .LogCommandResult(logger);

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

        private void DeleteProfileImages(Entities.ProfileImage profileImage)
        {
            try
            {
                if (profileImage != null)
                {
                    DeleteMedia(profileImage.FullView);
                    DeleteMedia(profileImage.Thumbnail);
                }
            }
            catch (Exception err)
            {
                logger.Error("Error deleting profile image: {0}", err);
            }
        }

        private void DeleteMedia(MediaData mediaData)
        {
            if (mediaData != null)
            {
                mediaManager.DeleteMedia(mediaData.MediaId);
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

        public void DeleteImage(string userName)
        {
            var profile = db.GetProfile(userName);
            DeleteProfileImages(profile.UserImage);

            var collection = db.GetCollection<Profile>(Profile.CollectionName);
            collection.Update(Query<Profile>.EQ(p => p.Id, profile.Id),
                              Update<Profile>.Unset(p => p.UserImage))
                      .LogCommandResult(logger);
        }

        public IEnumerable<MediaConstraint> GetMediaConstraints()
        {
            yield return new MediaConstraint { MediaType = MediaType.Jpeg, MaxFileSizeMB = config.MaxFileSizeMB };
        }
    }
}