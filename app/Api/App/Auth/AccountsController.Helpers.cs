using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using Api.App.Errors;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Shared.Extensions;

namespace Api.App.Auth
{
    [Authorize]
    public partial class AccountsController : ApiBaseController
    {
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
            if (ResultFailed(result))
            {
                if (result == null)
                {
                    var apiError = new ApiError
                    {
                        Code = ApiErrors.Auth.AuthError,
                        Message = string.Format(stringsProvider.GetString(ApiErrors.Auth.AuthError).AddFormatting(), "Failed creating IdentityResult")
                    };
                    logger.Error(apiError);
                    return ApiError(HttpStatusCode.InternalServerError, apiError);
                }
                else
                {
                    var apiError = new ApiError
                    {
                        Code = ApiErrors.Auth.AuthError,
                        Message = stringsProvider.GetString(ApiErrors.Auth.AuthError)
                    };
                    if (result.Errors != null)
                    {
                        apiError.Message = string.Format(apiError.Message.AddFormatting(), result.Errors.FirstOrDefault());
                    }

                    logger.Error(apiError);
                    return ApiError(HttpStatusCode.BadRequest, apiError);
                }
            }

            return Ok();
        }

        private static bool ResultFailed(IdentityResult result)
        {
            return result == null || !result.Succeeded;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }
            public string ExternalAccessToken { get; set; }
            public string AccessTokenSecret { get; set; }

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

                if (providerKeyClaim == null
                    || String.IsNullOrEmpty(providerKeyClaim.Issuer)
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
                    ExternalAccessToken = identity.FindFirstValue("ExternalAccessToken"),
                    AccessTokenSecret = identity.FindFirstValue("AccessTokenSecret")
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static readonly RandomNumberGenerator random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException(string.Format("strengthInBits must be evenly divisible by {0}.", bitsPerByte), "strengthInBits");
                }

                var strengthInBytes = strengthInBits / bitsPerByte;

                var data = new byte[strengthInBytes];
                random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }
    }
}