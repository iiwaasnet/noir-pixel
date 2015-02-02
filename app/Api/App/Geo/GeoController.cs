using System.Web.Http;
using Api.App.ApiBase;

namespace Api.App.Geo
{
    [RoutePrefix("geo")]
    public class GeoController : ApiBaseController
    {
        private readonly IGeoManager geoManager;
        public GeoController(IGeoManager geoManager)
        {
            this.geoManager = geoManager;
        }

        [HttpGet]
        [Route("countries")]
        public IHttpActionResult GetCountries()
        {
            return Ok(geoManager.GetCountries());
        }
    }
}