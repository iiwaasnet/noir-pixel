using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Api.Models;
using Microsoft.Owin.Security;

namespace Api.App.Users
{
    [Authorize]
    [RoutePrefix("users")]
    public class UsersController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private readonly ApplicationUserManager userManager;
        private readonly ISecureDataFormat<AuthenticationTicket> accessTokenFormat;

        public UsersController(ApplicationUserManager userManager)
        {
            this.userManager = userManager;
            this.accessTokenFormat = accessTokenFormat;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<IHttpActionResult> Register(RegisterRequest model)
        {
            //TODO: Model validation filter
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
            {
                return new ConflictResult(this);
            }

            var result = await userManager.CreateAsync(new ApplicationUser{UserName = model.UserName}, model.Password);

            //if (!result.Succeeded)
            //{
            //    return GetErrorResult(result);
            //}

            return Ok();
        }
    }
}