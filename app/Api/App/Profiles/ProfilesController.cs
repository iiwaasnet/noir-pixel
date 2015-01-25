﻿using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
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
        private readonly IMediaUploadManager mediaUploadManager;
        private readonly IProfileImageManager profileImageManager;

        public ProfilesController(IProfilesManager profilesManager,
                                  IMediaUploadManager mediaUploadManager,
                                  IProfileImageManager profileImageManager)
        {
            this.profilesManager = profilesManager;
            this.mediaUploadManager = mediaUploadManager;
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

                return Ok(profile);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        //TODO: Think of another controller for such data
        [Route("countries")]
        [AllowAnonymous]
        public IHttpActionResult GetCountries()
        {
            return Ok(profilesManager.GetCountries());
        }

        [HttpGet]
        [Route("update-profile-image")]
        public IHttpActionResult CheckProfileImagePart()
        {
            return mediaUploadManager.MediaChunkReceived(Request, User.Identity.Name)
                       ? Ok()
                       : ApiError(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("update-profile-image")]
        public async Task<IHttpActionResult> UpdateProfileImage()
        {
            try
            {
                var mediaUploadResult = await mediaUploadManager.ReceiveMediaChunk(Request, User.Identity.Name);
                if (mediaUploadResult.Completed)
                {
                    profileImageManager.SaveImage()
                }

                return Ok();
            }
            catch (NotSupportedException)
            {
                return ApiError(HttpStatusCode.NotAcceptable);
            }
        }
    }
}