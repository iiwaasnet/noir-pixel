using System.Web.Http;
using Api.App.ApiBase;

namespace Api.App.Photos
{
    [RoutePrefix("photos")]
    public class PhotosController : ApiBaseController
    {
        [HttpPost]
        [Route("upload")]
        public IHttpActionResult Upload()
        {
            return null;
        }
    }
}