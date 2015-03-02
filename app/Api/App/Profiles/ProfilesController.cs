﻿using System;
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
                    //TODO: try/catch and delete file in case of error
                    var url = profileImageManager.SaveImage(User.Identity.Name, mediaUploadResult.FileName);
                    url.FullViewUrl = MakeAbsoluteUrl(url.FullViewUrl);
                    url.ThumbnailUrl = MakeAbsoluteUrl(url.ThumbnailUrl);

                    mediaManager.DeleteMediaFile(mediaUploadResult.FileName);

                    return Ok(url);
                }

                return Ok();
            }
            catch (NotSupportedException)
            {
                return ApiError(HttpStatusCode.NotAcceptable);
            }
            catch (UnsupportedImageFormatException)
            {
                return ApiError(HttpStatusCode.UnsupportedMediaType, ApiErrors.Images.UnsupportedMediaFormat);
            }
            catch (OverMaxAllowedFileSizeException)
            {
                return ApiError(HttpStatusCode.BadRequest, ApiErrors.Images.FileTooBig);
            }
            catch (ImageSizeConstraintsException)
            {
                return ApiError(HttpStatusCode.BadRequest, ApiErrors.Images.ImageSizeViolation);
            }
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