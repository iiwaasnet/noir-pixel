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
            var includePrivateData = User.Identity.Self(userName);
            var profile = await profilesManager.GetUserProfile(userName, includePrivateData);

            profile.PublicInfo.ProfileImageThumbnail = MakeAbsoluteUrl(profile.PublicInfo.ProfileImageThumbnail);
            profile.PublicInfo.ProfileImage = MakeAbsoluteUrl(profile.PublicInfo.ProfileImage);

            return Ok(profile);
        }

        [HttpGet]
        [Route("update-profile-image")]
        public async Task<IHttpActionResult> CheckProfileImagePart()
        {
            return await mediaManager.MediaChunkReceived(Request, User.Identity.Name)
                       ? Ok()
                       : ApiError(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("update-profile/public-info")]
        public async Task<IHttpActionResult> UpdatePublicInfo(ProfilePublicInfo info)
        {
            await profilesManager.UpdatePublicInfo(User.Identity.Name, info);

            return Ok();
        }

        [HttpPost]
        [Route("update-profile/private-info")]
        public async Task<IHttpActionResult> UpdatePrivateInfo(ProfilePrivateInfo info)
        {
            await profilesManager.UpdatePrivateInfo(User.Identity.Name, info);

            return Ok();
        }

        [HttpPost]
        [Route("update-profile-image")]
        public async Task<IHttpActionResult> UpdateProfileImage()
        {
            var mediaUploadResult = await mediaManager.ReceiveMediaChunk(Request, User.Identity.Name);
            if (mediaUploadResult.Completed)
            {
                try
                {
                    var url = await SaveProfileImage(mediaUploadResult);
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

        private async Task<ProfileImage> SaveProfileImage(MediaUploadResult mediaUploadResult)
        {
            ProfileImage url = null;
            try
            {
                url = await profileImageManager.SaveImage(User.Identity.Name, mediaUploadResult.FileName);
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
        public async Task<IHttpActionResult> DeleteProfileImage()
        {
            await profileImageManager.DeleteImage(User.Identity.Name);

            return Ok();
        }
    }
}