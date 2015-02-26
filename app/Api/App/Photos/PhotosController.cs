using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Api.App.ApiBase;
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
        public IHttpActionResult CheckPhotosPart()
        {
            return mediaManager.MediaChunkReceived(Request, User.Identity.Name)
                       ? Ok()
                       : ApiError(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> Upload()
        {
            try
            {
                var mediaUploadResult = await mediaManager.ReceiveMediaChunk(Request, User.Identity.Name, photosManager.AssertFileSize);
                if (mediaUploadResult.Completed)
                {
                    var photo = photosManager.SavePhoto(User.Identity.Name, mediaUploadResult.FileName);
                    photo.FullViewUrl = MakeAbsoluteUrl(photo.FullViewUrl);
                    photo.PreviewUrl = MakeAbsoluteUrl(photo.PreviewUrl);
                    photo.ThumbnailUrl = MakeAbsoluteUrl(photo.ThumbnailUrl);

                    mediaManager.DeleteMediaFile(mediaUploadResult.FileName);

                    return Ok(photo);
                }

                return Ok();
            }
            catch (NotSupportedException)
            {
                return ApiError(HttpStatusCode.NotAcceptable);
            }
        }

        [HttpGet]
        [Route("pending")]
        public IHttpActionResult GetPendingPhotos(int? offset = null, int? count = null)
        {
            var pendingPhotos = photosManager.GetPendingPhotos(User.Identity.Name, offset, count);
            pendingPhotos.Photos = MakeAbsoluteUrl(pendingPhotos.Photos);

            return Ok(pendingPhotos);
        }

        private IEnumerable<Photo> MakeAbsoluteUrl(IEnumerable<Photo> photos)
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