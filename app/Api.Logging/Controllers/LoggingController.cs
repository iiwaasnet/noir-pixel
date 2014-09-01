using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.Logging.Controllers
{
    [RoutePrefix("logging")]
    public class LoggingController : ApiController
    {
        [Route("error")]
        public async Task<IHttpActionResult> Error()
        {
            return Ok();
        }
    }
}
