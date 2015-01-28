using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.App.Media
{
    [RoutePrefix(MediaManager.RoutePrefix)]
    public class MediaController : ApiBaseController
    {
        [HttpGet]
        [Route("{id}")]
        public Task<IHttpActionResult> Get(string id)
        {
            throw new Exception();
        }
    }
}