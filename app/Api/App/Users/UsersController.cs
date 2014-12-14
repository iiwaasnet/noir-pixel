using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace Api.App.Users
{
    [Authorize]
    [RoutePrefix("users")]
    public class UsersController : ApiBaseController
    {
        [AllowAnonymous]
        [Route("{userName}")]
        [HttpGet]
        public Task<IHttpActionResult> GetUserInfo(string userName)
        {
            if (User.Identity.IsAuthenticated && User.Identity.GetUserName().Equals(userName, StringComparison.InvariantCultureIgnoreCase))
            {
                var i = 1;
            }

            return null;
        }
    }
}