using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Api.App.ApiBase;
using Api.App.Images;
using Api.App.Media;

namespace Api.App.Photos
{
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
                var mediaUploadResult = await mediaManager.ReceiveMediaChunk(Request, User.Identity.Name);
                if (mediaUploadResult.Completed)
                {
                    var url = photosManager.SavePhoto(User.Identity.Name, mediaUploadResult.FileName);
                    url.FullViewUrl = MakeAbsoluteUrl(url.FullViewUrl);
                    url.PreviewUrl = MakeAbsoluteUrl(url.PreviewUrl);
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
        }
    }
}