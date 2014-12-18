using System.Net;
using System.Web.Http;
using Api.App.Db;
using Api.App.Users.Extensions;
using MongoDB.Driver;

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
            if (User.Identity.Self(userName))
            {
                var user = userManager.GetUserHome(userName);
            }

            return ApiError(HttpStatusCode.Forbidden);
        }
    }
}