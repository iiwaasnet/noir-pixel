using System;
using System.Linq;
using System.Net.Http;
using Api.App.Errors;

namespace Api.App.Auth.Extensions
{
    public static class HttpRequestExtensions
    {
        public static UriParseResult GetRedirectUri(this HttpRequestMessage request)
        {
            Uri redirectUri;

            var redirectUriString = request.GetQueryString("redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
            {
                return new UriParseResult
                       {
                           Parsed = false,
                           Error = new ValidationError
                                   {
                                       Code = ApiErrors.Validation.InvalidModelState,
                                       Message = "redirect_uri is required",
                                       Errors = new[] {new FieldValidationError {Code = ApiErrors.Validation.ValueRequired, Field = "redirect_uri"}}
                                   }
                       };
            }

            if (!Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri))
            {
                return new UriParseResult
                       {
                           Parsed = false,
                           Error = new ValidationError
                                   {
                                       Code = ApiErrors.Validation.InvalidModelState,
                                       Message = "redirect_uri is invalid",
                                       Errors = new[] {new FieldValidationError {Code = ApiErrors.Validation.InvalidValue, Field = "redirect_uri"}}
                                   }
                       };
            }

            return new UriParseResult {Uri = redirectUri.AbsoluteUri, Parsed = true};
        }

        private static string GetQueryString(this HttpRequestMessage request, string key)
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
    }
}