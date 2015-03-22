using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Api.App.ApiBase;
using Api.App.Errors;
using Api.App.Exceptions;
using Api.App.Images;
using Api.App.Media;

namespace Api.App.Photos
{
    [Authorize]
    [RoutePrefix("photos")]
    public class PhotosController : ApiBaseController
    {
        private readonly IMediaManager mediaManager;
        private readonly IPhotosManager photosManager;

        public PhotosController(IMediaManager mediaManager,
                                IPhotosManager photosManager)
        {
            this.mediaManager = mediaManager;
            this.photosManager = photosManager;
        }

        [HttpGet]
        [Route("upload")]
        public async Task<IHttpActionResult> CheckPhotosPart()
        {
            return await mediaManager.MediaChunkReceived(Request, User.Identity.Name)
                       ? Ok()
                       : ApiError(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> Upload()
        {
            try
            {
                var mediaUploadResult = await mediaManager.ReceiveMediaChunk(Request, User.Identity.Name);
                if (mediaUploadResult.Completed)
                {
                    try
                    {
                        var photo = await SavePhoto(mediaUploadResult);
                        photo.FullViewUrl = MakeAbsoluteUrl(photo.FullViewUrl);
                        photo.PreviewUrl = MakeAbsoluteUrl(photo.PreviewUrl);
                        photo.ThumbnailUrl = MakeAbsoluteUrl(photo.ThumbnailUrl);

                        return Ok(photo);
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

        private async Task<ImageData> SavePhoto(MediaUploadResult mediaUploadResult)
        {
            ImageData imageData = null;
            try
            {
                imageData = await photosManager.SavePhoto(User.Identity.Name, mediaUploadResult.FileName);
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

            return imageData;
        }

        [HttpGet]
        [Route("pending")]
        public async Task<IHttpActionResult> GetPendingPhotos(int? offset = null, int? count = null)
        {
            var pendingPhotos = await photosManager.GetPendingPhotos(User.Identity.Name, offset, count);
            pendingPhotos.Photos = MakeAbsoluteUrl(pendingPhotos.Photos);

            return Ok(pendingPhotos);
        }

        [HttpGet]
        [Route("{shortId}/edit")]
        public async Task<IHttpActionResult> GetPhotoForEdit(string shortId)
        {
            var photo = await photosManager.GetPhotoForEdit(User.Identity.Name, shortId);

            return null;
        }

        private IEnumerable<ImageData> MakeAbsoluteUrl(IEnumerable<ImageData> photos)
        {
            foreach (var photo in photos)
            {
                photo.FullViewUrl = MakeAbsoluteUrl(photo.FullViewUrl);
                photo.PreviewUrl = MakeAbsoluteUrl(photo.PreviewUrl);
                photo.ThumbnailUrl = MakeAbsoluteUrl(photo.ThumbnailUrl);

                yield return photo;
            }
        }
    }
}