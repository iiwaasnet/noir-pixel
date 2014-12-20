using System.Net;
using System.Web.Http;
using Api.App.Exceptions;
using Api.App.Users.Extensions;

namespace Api.App.Users
{
    [Authorize]
    [RoutePrefix("users")]
    public class UsersController : ApiBaseController
    {
        private readonly IUserManager userManager;

        public UsersController(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [Route("home/{userName}")]
        [HttpGet]
        public IHttpActionResult GetUserHome(string userName)
        {
            try
            {
                if (User.Identity.Self(userName))
                {
                    var user = userManager.GetUserHome(userName);

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