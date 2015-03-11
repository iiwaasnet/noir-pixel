using System.Net;
using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> Get(string id)
        {
            var link = await mediaManager.GetMediaLink(id);
            if (link != null)
            {
                if (link.Remote)
                {
                    return Redirect(link.Location);
                }

                return new FileResult(link.Location);
            }

            return ApiError(HttpStatusCode.NotFound);
        }
    }
}