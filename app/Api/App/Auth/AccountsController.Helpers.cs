using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using Api.App.ApiBase;
using Api.App.Auth.Extensions;
using Api.App.Errors;
using Api.App.Errors.Extensions;
using Common.Extensions;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Api.App.Auth
{
    [Authorize]
    public partial class AccountsController : ApiBaseController
    {
        private IAuthenticationManager GetAuthentication()
        {
            return Request.GetOwinContext().Authentication;
        }

        private void AssertIdentityResult(IdentityResult result, ApplicationUser user)
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
                    ApiException(HttpStatusCode.InternalServerError, apiError);
                }
                else
                {
                    var apiError = CreateApiError(result, user);

                    ApiException(GetHttpErrorCode(apiError.Code), apiError);
                }
            }
        }

        private static bool ResultFailed(IdentityResult result)
        {
            return result == null || !result.Succeeded;
        }

        private HttpStatusCode GetHttpErrorCode(string code)
        {
            switch (code)
            {
                case ApiErrors.Auth.UserAlreadyRegistered:
                    return HttpStatusCode.Conflict;
                default:
                    return HttpStatusCode.BadRequest;
            }
        }

        private ApiError CreateApiError(IdentityResult result, ApplicationUser user)
        {
            var apiError = new ApiError
                           {
                               Code = ApiErrors.Auth.AuthError,
                               Message = stringsProvider.GetString(ApiErrors.Auth.AuthError)
                           };

            if (result.Errors != null && !result.Errors.Any(string.IsNullOrWhiteSpace))
            {
                var errorMessage = result.Errors.First(e => !string.IsNullOrWhiteSpace(e));
                apiError.Message = errorMessage;

                if (errorMessage.Contains("is already taken"))
                {
                    apiError.Code = ApiErrors.Auth.UserAlreadyRegistered;
                    apiError.PlaceholderValues = new Dictionary<string, object> {{"UserName", user.UserName}};
                }
            }

            return apiError;
        }

        private void AssertModelStateValid()
        {
            if (!ModelState.IsValid)
            {
                var validationError = ModelState.ToValidationError(stringsProvider);

                ApiException(HttpStatusCode.BadRequest, validationError);
            }
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