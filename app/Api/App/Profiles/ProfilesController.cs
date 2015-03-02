using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Api.App.ApiBase;
using Api.App.Errors;
using Api.App.Exceptions;
using Api.App.Images;
using Api.App.Media;
using Api.App.Profiles.Extensions;

namespace Api.App.Profiles
{
    [Authorize]
    [RoutePrefix("profiles")]
    public class ProfilesController : ApiBaseController
    {
        private readonly IProfilesManager profilesManager;
        private readonly IMediaManager mediaManager;
        private readonly IProfileImageManager profileImageManager;

        public ProfilesController(IProfilesManager profilesManager,
                                  IMediaManager mediaManager,
                                  IProfileImageManager profileImageManager)
        {
            this.profilesManager = profilesManager;
            this.mediaManager = mediaManager;
            this.profileImageManager = profileImageManager;
        }

        [Route("{userName}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(string userName)
        {
            try
            {
                var includePrivateData = User.Identity.Self(userName);
                var profile = await profilesManager.GetUserProfile(userName, includePrivateData);

                profile.PublicInfo.ProfileImageThumbnail = MakeAbsoluteUrl(profile.PublicInfo.ProfileImageThumbnail);
                profile.PublicInfo.ProfileImage = MakeAbsoluteUrl(profile.PublicInfo.ProfileImage);

                return Ok(profile);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("update-profile-image")]
        public IHttpActionResult CheckProfileImagePart()
        {
            return mediaManager.MediaChunkReceived(Request, User.Identity.Name)
                       ? Ok()
                       : ApiError(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("update-profile/public-info")]
        public IHttpActionResult UpdatePublicInfo(ProfilePublicInfo info)
        {
            try
            {
                profilesManager.UpdatePublicInfo(User.Identity.Name, info);

                return Ok();
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("update-profile/private-info")]
        public IHttpActionResult UpdatePrivateInfo(ProfilePrivateInfo info)
        {
            try
            {
                profilesManager.UpdatePrivateInfo(User.Identity.Name, info);

                return Ok();
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPost]
        [Route("update-profile-image")]
        public async Task<IHttpActionResult> UpdateProfileImage()
        {
            try
            {
                var mediaUploadResult = await mediaManager.ReceiveMediaChunk(Request, User.Identity.Name);
                if (mediaUploadResult.Completed)
                {
                    try
                    {
                        var url = SaveProfileImage(mediaUploadResult);
                        url.FullViewUrl = MakeAbsoluteUrl(url.FullViewUrl);
                        url.ThumbnailUrl = MakeAbsoluteUrl(url.ThumbnailUrl);

                        return Ok(url);
                    }
                    finally
                    {
                        mediaManager.DeleteMediaFile(mediaUploadResult.FileName);
                    }
                }

                return Ok();
            }
            catch (NotSupportedException)
            {
                return ApiError(HttpStatusCode.NotAcceptable);
            }
        }

        private ProfileImage SaveProfileImage(MediaUploadResult mediaUploadResult)
        {
            ProfileImage url = null;
            try
            {
                url = profileImageManager.SaveImage(User.Identity.Name, mediaUploadResult.FileName);
            }
            catch (UnsupportedImageFormatException)
            {
                ApiException(HttpStatusCode.UnsupportedMediaType, ApiErrors.Images.UnsupportedMediaFormat);
            }
            catch (OverMaxAllowedFileSizeException)
            {
                ApiException(HttpStatusCode.BadRequest, ApiErrors.Images.FileTooBig);
            }
            catch (ImageSizeConstraintsException)
            {
                ApiException(HttpStatusCode.BadRequest, ApiErrors.Images.ImageSizeViolation);
            }

            return url;
        }

        [HttpDelete]
        [Route("delete-profile-image")]
        public IHttpActionResult DeleteProfileImage()
        {
            profileImageManager.DeleteImage(User.Identity.Name);

            return Ok();
        }
    }
}