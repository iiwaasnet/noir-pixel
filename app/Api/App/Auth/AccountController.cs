using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using Api.App.Auth.ExternalUserInfo;
using Api.Models;
using Api.Providers;
using Api.Results;
using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebGrease.Css.Extensions;

namespace Api.App.Auth
{
    [Authorize]
    [RoutePrefix("account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private readonly ApplicationUserManager userManager;
        private readonly AuthOptions authOptions;
        private readonly IExternalAccountsManager externalAccountsManager;

        public AccountController(ApplicationUserManager userManager, AuthOptions authOptions, IExternalAccountsManager externalAccountsManager)
        {
            this.userManager = userManager;
            this.authOptions = authOptions;
            this.externalAccountsManager = externalAccountsManager;
        }

        [AllowAnonymous]
        [Route("register")]
        public async Task<IHttpActionResult> Register(RegisterModel model)
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

            var result = await userManager.CreateAsync(new ApplicationUser
                                                       {
                                                           UserName = model.UserName,
                                                           Email = model.Email
                                                       },
                                                       model.Password);

            return GetIdentityResult(result);
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("external-login", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                GetAuthentication().SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            var user = await userManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                                                                     externalLogin.ProviderKey));

            var registered = user != null;

            //TODO: Refactor. GetRedirectUri() should be called earlier with other checks to return proper error http response 
            var redirectUri = string.Format("{0}#external_access_token={1}&registered={2}",
                                            GetRedirectUri(Request),
                                            externalLogin.ExternalAccessToken,
                                            registered.ToString().ToLower());

            return Redirect(redirectUri);
        }

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [AllowAnonymous]
        [Route("register-external")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifiedAccessToken = await externalAccountsManager.VerfiyAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }            

            var user = await userManager.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

            var hasRegistered = user != null;

            if (hasRegistered)
            {
                return BadRequest("External user is already registered");
            }

            var externalUserInfo = await externalAccountsManager.GetUserInfo(model.Provider, verifiedAccessToken.user_id, model.ExternalAccessToken);

            user = new ApplicationUser
                   {
                       UserName = CreateUserName(externalUserInfo.Person.DisplayName),
                       Email = externalUserInfo.Emails.First(m => m.Type == "account").Address
                   };

            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetIdentityErrorResult(result);
            }

            result = await userManager.AddLoginAsync(user.Id, new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));
            if (!result.Succeeded)
            {
                return GetIdentityErrorResult(result);
            }

            return Ok(new JObject(new JProperty("access_token", model.ExternalAccessToken)));
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
            if (string.IsNullOrWhiteSpace(model.Provider) || string.IsNullOrWhiteSpace(model.ExternalAccessToken))
            {
                return BadRequest("Provider or external access token is not sent");
            }

            var verifiedAccessToken = await externalAccountsManager.VerfiyAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
            {
                return BadRequest("Invalid Provider or External Access Token");
            }

            var user = await userManager.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

            var hasRegistered = user != null;

            if (hasRegistered)
            {
                GetAuthentication().SignOut(DefaultAuthenticationTypes.ExternalCookie);

                var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager,
                                                                         OAuthDefaults.AuthenticationType);
                var cookieIdentity = await user.GenerateUserIdentityAsync(userManager,
                                                                          CookieAuthenticationDefaults.AuthenticationType);

                var properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                GetAuthentication().SignIn(properties, oAuthIdentity, cookieIdentity);

                var ticket = new AuthenticationTicket(oAuthIdentity, properties);

                var accessToken = authOptions.AuthServerOptions.AccessTokenFormat.Protect(ticket);

                var tokenResponse = new JObject(
                    new JProperty("userName", user.UserName),
                    new JProperty("access_token", accessToken),
                    new JProperty("token_type", "bearer"),
                    //new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                    new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                    new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
                    );
                return Ok(tokenResponse);
            }

            return BadRequest("User is not registered!");
        }

        private string GetRedirectUri(HttpRequestMessage request)
        {
            Uri redirectUri;

            var redirectUriString = GetQueryString(request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return "redirect_uri is required";
            }

            if (!Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri))
            {
                return "redirect_uri is invalid";
            }

            return redirectUri.AbsoluteUri;
        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings != null)
            {
                var match = queryStrings.FirstOrDefault(keyValue => String.Compare(keyValue.Key, key, StringComparison.OrdinalIgnoreCase) == 0);

                if (!string.IsNullOrWhiteSpace(match.Value))
                {
                    return match.Value;
                }
            }

            return null;
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

        [Route("logout")]
        public IHttpActionResult Logout()
        {
            GetAuthentication().SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
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

        // GET account/external-login

        

        [AllowAnonymous]
        [Route("external-logins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            var descriptions = GetAuthentication().GetExternalAuthenticationTypes();
            var logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (var description in descriptions)
            {
                var login = new ExternalLoginViewModel
                            {
                                Name = description.Caption,
                                Url = Url.Route("ExternalLogin",
                                                new
                                                {
                                                    provider = description.AuthenticationType,
                                                    response_type = "token",
                                                    client_id = authOptions.PublicClientId,
                                                    redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                                                    state = state
                                                }),
                                State = state
                            };
                logins.Add(login);
            }

            return logins;
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            var endPoint = "";

            if (provider == "Facebook")
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                var appToken = "xxxxxx";
                endPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
            }
            else if (provider == "GooglePlus")
            {
                endPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(endPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == "Facebook")
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    //TODO: Uncomment and fix!!!!
                    //if (!string.Equals(Startup.facebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    //{
                    //    return null;
                    //}
                }
                else if (provider == "GooglePlus")
                {
                    parsedToken.user_id = jObj.user_id;
                    parsedToken.app_id = jObj.audience;
                    parsedToken.email = jObj.email;

                    if (!string.Equals(authOptions.GooglePlusAuthOptions.ClientId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
            }

            //TODO: If response.IsSuccessStatusCode == false, then re-authentication needed
            return parsedToken;
        }

        //private async Task<ExternalUserInfo.ExternalUserInfo> GetExternalUserInfo(string provider, string user_id, string accessToken)
        //{
        //    ExternalUserInfo.ExternalUserInfo token = null;

        //    var endPoint = "";

        //    if (provider == "Facebook")
        //    {
        //        //You can get it from here: https://developers.facebook.com/tools/accesstoken/
        //        //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
        //        var appToken = "xxxxxx";
        //        endPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, appToken);
        //    }
        //    else if (provider == "GooglePlus")
        //    {
        //        endPoint = string.Format("https://www.googleapis.com/plus/v1/people/{0}?access_token={1}", user_id, accessToken);
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //    var client = new HttpClient();
        //    var uri = new Uri(endPoint);
        //    var response = await client.GetAsync(uri);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var content = await response.Content.ReadAsStringAsync();

        //        dynamic jObj = JsonConvert.DeserializeObject(content);

        //        token = new ExternalUserInfo.ExternalUserInfo();

        //        //if (provider == "Facebook")
        //        //{
        //        //    token.user_id = jObj["data"]["user_id"];
        //        //    token.app_id = jObj["data"]["app_id"];

        //        //    //TODO: Uncomment and fix!!!!
        //        //    //if (!string.Equals(Startup.facebookAuthOptions.AppId, parsedToken.app_id, StringComparison.OrdinalIgnoreCase))
        //        //    //{
        //        //    //    return null;
        //        //    //}
        //        //}

        //        if (provider == "GooglePlus")
        //        {
        //            token.Person = new PersonInfo
        //                           {
        //                               Id = jObj.id,
        //                               DisplayName = jObj.displayName,
        //                               FirstName = jObj.name.givenName,
        //                               LastName = jObj.name.familyName,
        //                               Gender = jObj.gender,
        //                               Image = jObj.image.url
        //                           };
        //            token.Emails = GetEmails(jObj.emails);
        //            //token.Links;

        //            if (!string.Equals(user_id, token.Person.Id, StringComparison.OrdinalIgnoreCase))
        //            {
        //                return null;
        //            }
        //        }
        //    }

        //    //TODO: If response.IsSuccessStatusCode == false, then re-authentication needed
        //    return token;
        //}

        

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

        #region Helpers

        private IAuthenticationManager GetAuthentication()
        {
            return Request.GetOwinContext().Authentication;
        }

        private IHttpActionResult GetIdentityResult(IdentityResult result)
        {
            return (result.Succeeded)
                       ? Ok()
                       : GetIdentityErrorResult(result);
        }

        private IHttpActionResult GetIdentityErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    result.Errors.ForEach(e => ModelState.AddModelError("", e));
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }
            public string ExternalAccessToken { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                var providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                       {
                           LoginProvider = providerKeyClaim.Issuer,
                           ProviderKey = providerKeyClaim.Value,
                           UserName = identity.FindFirstValue(ClaimTypes.Name),
                           ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken")
                       };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static readonly RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                var strengthInBytes = strengthInBits / bitsPerByte;

                var data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}