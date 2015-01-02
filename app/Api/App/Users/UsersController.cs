using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Api.App.Exceptions;
using Api.App.Users.Extensions;

namespace Api.App.Users
{
    [Authorize]
    [RoutePrefix("users")]
    public class UsersController : ApiBaseController
    {
        private readonly IUsersManager usersManager;

        public UsersController(IUsersManager usersManager)
        {
            this.usersManager = usersManager;
        }

        [Route("home/{userName}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserHome(string userName)
        {
            try
            {
                if (User.Identity.Self(userName))
                {
                    var user = await usersManager.GetUserHome(userName);

                    return Ok(user);
                }

                return ApiError(HttpStatusCode.Forbidden);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [Route("profile/{userName}")]
        [HttpGet]
        public IHttpActionResult GetUserProfile(string userName)
        {
            var includePrivateData = User.Identity.Self(userName);
            var profile = usersManager.GetUserProfile(userName, includePrivateData);

            return null;
        }
    }
}