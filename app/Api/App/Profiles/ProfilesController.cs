using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Api.App.Exceptions;
using Api.App.Profiles.Entities;
using Api.App.Profiles.Extensions;

namespace Api.App.Profiles
{
    [Authorize]
    [RoutePrefix("profiles")]
    public class ProfilesController : ApiBaseController
    {
        private readonly IProfilesManager profilesManager;

        public ProfilesController(IProfilesManager profilesManager)
        {
            this.profilesManager = profilesManager;
        }

        [Route("{userName}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string userName)
        {
            try
            {
                var includePrivateData = User.Identity.Self(userName);
                var profile = await profilesManager.GetUserProfile(userName, includePrivateData);

                return Ok(profile);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}