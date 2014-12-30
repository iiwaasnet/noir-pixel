using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Api.App.Artists.Extensions;
using Api.App.Exceptions;

namespace Api.App.Artists
{
    [Authorize]
    [RoutePrefix("artists")]
    public class ArtistsController : ApiBaseController
    {
        private readonly IArtistManager artistManager;

        public ArtistsController(IArtistManager artistManager)
        {
            this.artistManager = artistManager;
        }

        [Route("home/{userName}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserHome(string userName)
        {
            try
            {
                if (User.Identity.Self(userName))
                {
                    var user = await artistManager.GetUserHome(userName);

                    return Ok(user);
                }

                return ApiError(HttpStatusCode.Forbidden);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}