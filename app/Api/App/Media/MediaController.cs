using System.Web.Http;
using Api.App.ApiBase;

namespace Api.App.Media
{
    [RoutePrefix(MediaManager.RoutePrefix)]
    public class MediaController : ApiBaseController
    {
        private readonly IMediaManager mediaManager;

        public MediaController(IMediaManager mediaManager)
        {
            this.mediaManager = mediaManager;
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            var link = mediaManager.GetMediaLink(id);
            if (link.Remote)
            {
                return Redirect(link.Location);
            }

            return Ok();
        }
    }
}