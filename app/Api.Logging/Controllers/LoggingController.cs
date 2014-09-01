﻿using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Api.Logging.Models;

namespace Api.Logging.Controllers
{
    [RoutePrefix("logging")]
    [EnableCors(origins: "http://noir-pixel.com", headers: "*", methods: "GET, POST")]
    public class LoggingController : ApiController
    {
        [Route("error")]
        public async Task<IHttpActionResult> Error(ApplicationLoggingModel error)
        {
            return Ok();
        }
        
        [Route("debug")]
        public async Task<IHttpActionResult> Debug(ApplicationLoggingModel debug)
        {
            return Ok();
        }
    }
}