using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Api.App.Auth.Extensions;
using Api.App.Auth.ExternalUserInfo;
using Api.App.Errors;
using Api.App.Errors.Extensions;
using Api.Models;
using Api.Results;
using AspNet.Identity.MongoDB;
using Diagnostics;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;
using Resources.Api;
using Shared.Extensions;

namespace Api.App.Auth
{
    [Authorize]
    [RoutePrefix("accounts")]
    public partial class AccountsController : ApiBaseController
    {
        private const string LocalLoginProvider = "Local";
        private readonly ApplicationUserManager userManager;
        private readonly AuthOptions authOptions;
        private readonly IExternalAccountsManager externalAccountsManager;
        private readonly IApiStringsProvider stringsProvider;
        private readonly ILogger logger;

        public AccountsController(ApplicationUserManager userManager,
                                  AuthOptions authOptions,
                                  IExternalAccountsManager externalAccountsManager,
                                  IApiStringsProvider stringsProvider,
                                  ILogger logger)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.authOptions = authOptions;
            this.stringsProvider = stringsProvider;
            this.externalAccountsManager = externalAccountsManager;
        }

        //[AllowAnonymous]
        //[Route("register")]
        //public async Task<IHttpActionResult> Register(RegisterModel model)
        //{
        //    var result = CheckModelState();
        //    if (!result.Succeeded)
        //    {
        //        return result.Error;
        //    }

        //    var existingUser = await userManager.FindByNameAsync(model.UserName);
        //    if (existingUser != null)
        //    {
        //        return new ConflictResult(this);
        //    }

        //    var identityResult = await userManager.CreateAsync(new ApplicationUser
        //                                               {
        //                                                   UserName = model.UserName,
        //                                                   Email = model.Email
        //                                               },
        //                                               model.Password);

        //    return GetIdentityResult(identityResult);
        //}

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("external-login", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            var result = ParseRedirectUrl();
            if (!result.Succeeded)
            {
                return result.Error;
            }
            var redirectUri = result.Data.Uri;

            if (error != null)
            {
                logger.Error(ApiErrors.Auth.AuthError.AddFormatting(), error);
                return RedirectWithError(redirectUri, error);
            }
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return new ChallengeResult(provider, this);
                }

                var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);
                if (externalLogin == null)
                {
                    logger.Error(stringsProvider.GetString(ApiErrors.Auth.ExternalLoginDataNotFound).AddFormatting(), User.Identity);
                    return RedirectWithError(redirectUri, ApiErrors.Auth.ExternalLoginDataNotFound);
                }

                if (externalLogin.LoginProvider != provider)
                {
                    GetAuthentication().SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    return new ChallengeResult(provider, this);
                }

                var user = await userManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                                                                         externalLogin.ProviderKey));

                var registered = user != null;

                redirectUri = string.Format("{0}#external_access_token={1}&access_token_secret={2}&registered={3}&provider={4}",
                                            redirectUri,
                                            externalLogin.ExternalAccessToken,
                                            externalLogin.AccessTokenSecret,
                                            registered.ToString().ToLower(),
                                            provider);

                return Redirect(redirectUri);
            }
            catch (Exception err)
            {
                logger.Error(err);
                return RedirectWithError(redirectUri, ApiErrors.InternalError);
            }
        }

        private MethodExecutionResult<UriParseResult> ParseRedirectUrl()
        {
            var redirectUriResult = Request.GetRedirectUri();
            if (!redirectUriResult.Parsed)
            {
                logger.Error(ApiErrors.Auth.AuthError.AddFormatting(), redirectUriResult.Error);
                return new MethodExecutionResult<UriParseResult>(ApiError(HttpStatusCode.BadRequest, redirectUriResult.Error));
            }

            return new MethodExecutionResult<UriParseResult>(redirectUriResult);
        }

        private IHttpActionResult RedirectWithError(string redirectUri, string error)
        {
            return Redirect(string.Format("{0}#error={1}", redirectUri, Uri.EscapeDataString(error)));
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [AllowAnonymous]
        [Route("register-external")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalModel model)
        {
            var result = CheckModelState();
            if (!result.Succeeded)
            {
                return result.Error;
            }

            var verifiedAccessToken = await externalAccountsManager.VerfiyAccessToken(model.Provider, model.ExternalAccessToken, model.AccessTokenSecret);
            if (verifiedAccessToken == null)
            {
                var apiError = new ApiError
                               {
                                   Code = ApiErrors.Auth.InvalidProviderOrAccessToken,
                                   Message = stringsProvider.GetString(ApiErrors.Auth.InvalidProviderOrAccessToken)
                               };
                logger.Error(apiError);
                return ApiError(HttpStatusCode.BadRequest, apiError);
            }

            var user = await userManager.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));
            var hasRegistered = user != null;
            if (hasRegistered)
            {
                var apiError = new ApiError
                               {
                                   Code = ApiErrors.Auth.UserAlreadyRegistered,
                                   Message = string.Format(stringsProvider.GetString(ApiErrors.Auth.UserAlreadyRegistered)
                                                                          .AddFormatting(2),
                                                           model.Provider,
                                                           verifiedAccessToken.user_id)
                               };
                logger.Error(apiError);
                return ApiError(HttpStatusCode.Conflict, apiError);
            }

            var externalUserInfo = await externalAccountsManager.GetUserInfo(model.Provider, verifiedAccessToken.user_id, model.ExternalAccessToken, model.AccessTokenSecret);
            user = new ApplicationUser
                   {
                       UserName = CreateUserName(externalUserInfo.Person.DisplayName),
                       Email = externalUserInfo.Email
                   };

            var identityResult = await userManager.CreateAsync(user);
            if (!identityResult.Succeeded)
            {
                return GetIdentityErrorResult(identityResult);
            }

            identityResult = await userManager.AddLoginAsync(user.Id, new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));
            if (!result.Succeeded)
            {
                return GetIdentityErrorResult(identityResult);
            }

            return Ok(new
                      {
                          access_token = model.ExternalAccessToken,
                          access_token_secret = model.AccessTokenSecret,
                          provider = model.Provider
                      });
        }

        private string CreateUserName(string displayName)
        {
            return displayName.ToLower().Replace(' ', '-');
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [HttpPost]
        [Route("local-access-token")]
        public async Task<IHttpActionResult> LocalAccessToken(LocalAccessTokenModel model)
        {
            var result = CheckModelState();
            if (!result.Succeeded)
            {
                return result.Error;
            }

            var verifiedAccessToken = await externalAccountsManager.VerfiyAccessToken(model.Provider, model.ExternalAccessToken, model.AccessTokenSecret);
            if (verifiedAccessToken == null)
            {
                var error = new ApiError
                            {
                                Code = ApiErrors.Auth.InvalidExternalAccessToken,
                                Message = stringsProvider.GetString(ApiErrors.Auth.InvalidExternalAccessToken)
                            };
                logger.Error(error);
                return ApiError(HttpStatusCode.BadRequest, error);
            }

            var user = await userManager.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

            var hasRegistered = user != null;

            if (hasRegistered)
            {
                GetAuthentication().SignOut(DefaultAuthenticationTypes.ExternalCookie);

                var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
                var cookieIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);

                var expiresIn = authOptions.AuthServerOptions.AccessTokenExpireTimeSpan;
                var properties = CreateAuthenticationProperties(user, expiresIn);
                GetAuthentication().SignIn(properties, oAuthIdentity, cookieIdentity);

                var ticket = new AuthenticationTicket(oAuthIdentity, properties);

                var accessToken = authOptions.AuthServerOptions.AccessTokenFormat.Protect(ticket);

                var tokenResponse = new JObject(
                    new JProperty("userName", user.UserName),
                    new JProperty("access_token", accessToken),
                    new JProperty("token_type", "bearer"),
                    new JProperty("expires_in", expiresIn.TotalSeconds.ToString()),
                    new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                    new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
                    );
                return Ok(tokenResponse);
            }
            else
            {
                var error = new ApiError
                            {
                                Code = ApiErrors.Auth.UserNotRegistered,
                                Message = string.Format(stringsProvider.GetString(ApiErrors.Auth.UserNotRegistered).AddFormatting(2),
                                                        model.Provider,
                                                        verifiedAccessToken.user_id)
                            };
                logger.Error(error);
                return ApiError(HttpStatusCode.NotFound, error);
            }
        }

        private MethodExecutionResult<bool> CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                var validationError = ModelState.ToValidationError(stringsProvider);
                logger.Error(validationError);
                return new MethodExecutionResult<bool>(ApiError(HttpStatusCode.BadRequest, validationError));
            }

            return new MethodExecutionResult<bool>(true);
        }

        private static AuthenticationProperties CreateAuthenticationProperties(ApplicationUser user, TimeSpan expiresIn)
        {
            return new AuthenticationProperties(new Dictionary<string, string>
                                                {
                                                    {"userName", user.UserName}
                                                })
                   {
                       IssuedUtc = DateTime.UtcNow,
                       ExpiresUtc = DateTime.UtcNow.Add(expiresIn)
                   };
        }

        [AllowAnonymous]
        [Route("external-logins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            var state = GenerateState(generateState);

            return GetAuthentication()
                .GetExternalAuthenticationTypes()
                .Select(p => CerateLoginProviderDescription(returnUrl, p, state))
                .ToArray();
        }

        [Route("logout")]
        public IHttpActionResult Logout()
        {
            try
            {
                GetAuthentication().SignOut(CookieAuthenticationDefaults.AuthenticationType);
            }
            catch (Exception err)
            {
                logger.Error(err);
            }

            return Ok();
        }

        private ExternalLoginViewModel CerateLoginProviderDescription(string returnUrl, AuthenticationDescription p, string state)
        {
            return new ExternalLoginViewModel
                   {
                       Name = p.Caption,
                       Url = Url.Route("ExternalLogin",
                                       new
                                       {
                                           provider = p.AuthenticationType,
                                           response_type = "token",
                                           client_id = authOptions.PublicClientId,
                                           redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                                           state = state
                                       }),
                       State = state
                   };
        }

        private static string GenerateState(bool generateState)
        {
            const int strengthInBits = 256;

            return (generateState)
                       ? RandomOAuthStateGenerator.Generate(strengthInBits)
                       : null;
        }

        //===========================================================================================
        //===========================================================================================
        //===========================================================================================

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("user-info")]
        public UserInfoViewModel GetUserInfo()
        {
            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
                   {
                       Email = User.Identity.GetUserName(),
                       HasRegistered = externalLogin == null,
                       LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
                   };
        }

        //GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await userManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            var logins = new List<UserLoginInfo>();

            foreach (var linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfo(
                               loginProvider: linkedAccount.LoginProvider,
                               providerKey: linkedAccount.ProviderKey
                               ));
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfo
                               (
                               loginProvider: LocalLoginProvider,
                               providerKey: user.UserName
                               ));
            }

            return new ManageInfoViewModel
                   {
                       LocalLoginProvider = LocalLoginProvider,
                       Email = user.UserName,
                       Logins = logins,
                       ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
                   };
        }

        [Route("change-password")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await userManager.ChangePasswordAsync(User.Identity.GetUserId(),
                                                               model.OldPassword,
                                                               model.NewPassword);

            return GetIdentityResult(result);
        }

        [Route("set-password")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await userManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            return GetIdentityResult(result);
        }

        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GetAuthentication().SignOut(DefaultAuthenticationTypes.ExternalCookie);

            var ticket = authOptions.AuthServerOptions.AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                                                              && ticket.Properties.ExpiresUtc.HasValue
                                                              && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            var externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            var result = await userManager.AddLoginAsync(User.Identity.GetUserId(),
                                                         new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            return GetIdentityResult(result);
        }

        [Route("remove-login")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await userManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await userManager.RemoveLoginAsync(User.Identity.GetUserId(),
                                                            new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            return GetIdentityResult(result);
        }

        private static ExternalLoginInfo GetExternalLoginInfo(AuthenticateResult result)
        {
            if (result == null || result.Identity == null)
            {
                return null;
            }
            var idClaim = result.Identity.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null)
            {
                return null;
            }
            // By default we don't allow spaces in user names
            var name = result.Identity.Name;
            if (name != null)
            {
                name = name.Replace(" ", "");
            }
            var email = result.Identity.FindFirstValue(ClaimTypes.Email);
            return new ExternalLoginInfo
                   {
                       ExternalIdentity = result.Identity,
                       Login = new UserLoginInfo(idClaim.Issuer, idClaim.Value),
                       DefaultUserName = name,
                       Email = email
                   };
        }
    }
}