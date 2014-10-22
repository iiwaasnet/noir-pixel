using System.Threading.Tasks;
using System.Web.Http;
using Api.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Api.App.Users
{
    [Authorize]
    [RoutePrefix("users")]
    public class UsersController : ApiController
    {
        private readonly ApplicationUserManager userManager;
        private readonly ISecureDataFormat<AuthenticationTicket> accessTokenFormat;

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager,
                               ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            this.userManager = userManager;
            this.accessTokenFormat = accessTokenFormat;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser { UserName = model.UserName };

            var result = await userManager.CreateAsync(user, model.Password);

            //if (!result.Succeeded)
            //{
            //    return GetErrorResult(result);
            //}

            return Ok();
        }
    }
}