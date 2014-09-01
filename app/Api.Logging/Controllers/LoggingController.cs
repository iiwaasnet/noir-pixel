using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Api.Logging.Models;
using Newtonsoft.Json.Serialization;

namespace Api.Logging.Controllers
{
    [RoutePrefix("logging")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoggingController : ApiController
    {
        [Route("error")]
        public async Task<IHttpActionResult> Error(ErrorLoggingModel error)
        {
            return Ok();
        }
    }
}