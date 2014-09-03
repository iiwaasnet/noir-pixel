using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Api.Logging.Models;

namespace Api.Logging.Controllers
{
    [RoutePrefix("log")]
    [EnableCors(origins: "http://noir-pixel.com", headers: "*", methods: "GET, POST")]
    public class LoggingController : ApiController
    {
        [Route("add")]
        public async Task<IHttpActionResult> Add(ApplicationLoggingModel entry)
        {
            return Ok();
        }
    }
}