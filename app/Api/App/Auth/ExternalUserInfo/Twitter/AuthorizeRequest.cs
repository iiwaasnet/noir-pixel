using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Owin.Security.Twitter;

namespace Api.App.Auth.ExternalUserInfo.Twitter
{
    public class AuthorizeRequest
    {
        private readonly TwitterAuthenticationOptions authOptions;

        public AuthorizeRequest(TwitterAuthenticationOptions authOptions)
        {
            this.authOptions = authOptions;
        }

        public string CreateAuthHeader(string method, string baseUrl, string authToken, string accessTokenSecret, IEnumerable<KeyValuePair<string, string>> requestParams)
        {
            var sortedParams = GetDefaultAuthHeaderParams(authToken);
            var signature = SignRequest(method, baseUrl, authToken, accessTokenSecret, requestParams);
            sortedParams["oauth_signature"] = signature;

            return string.Join(", ",
                               sortedParams
                                   .OrderBy(p => p.Key)
                                   .Select(p => string.Format("{0}=\"{1}\"",
                                                              Uri.EscapeDataString(p.Key),
                                                              Uri.EscapeDataString(p.Value))));
        }

        private string SignRequest(string method, string baseUrl, string authToken, string accessTokenSecret, IEnumerable<KeyValuePair<string, string>> requestParams)
        {
            var signBase = string.Format("{0}&{1}&{2}",
                                         method.ToUpper(),
                                         Uri.EscapeDataString(baseUrl),
                                         Uri.EscapeDataString(CreateRequestSignature(authToken, requestParams)));

            var hmacsha1 = new HMACSHA1(new ASCIIEncoding().GetBytes(CreateSigningKey(accessTokenSecret)));

            var signatureString = Convert.ToBase64String(hmacsha1.ComputeHash(new ASCIIEncoding().GetBytes(signBase)));

            return signatureString;
        }

        private string CreateRequestSignature(string authToken, IEnumerable<KeyValuePair<string, string>> requestParams)
        {
            var sorted = new SortedDictionary<string, string>(GetDefaultAuthHeaderParams(authToken));
            foreach (var requestParam in requestParams)
            {
                sorted[Uri.EscapeDataString(requestParam.Key)] = Uri.EscapeDataString(requestParam.Value);
            }

            return CreateParameterString(sorted);
        }

        private IDictionary<string, string> GetDefaultAuthHeaderParams(string authToken)
        {
            return new Dictionary<string, string>
                   {
                       {"oauth_consumer_key", Uri.EscapeDataString(authOptions.ConsumerKey)},
                       {"oauth_nonce", Uri.EscapeDataString(GenerateNonce())},
                       {"oauth_signature_method", Uri.EscapeDataString("HMAC-SHA1")},
                       {"oauth_timestamp", Uri.EscapeDataString(GenerateTimestamp())},
                       {"oauth_token", Uri.EscapeDataString(authToken)},
                       {"oauth_version", Uri.EscapeDataString("1.0")}
                   };
        }

        private string CreateSigningKey(string accessTokenSecret)
        {
            return string.Format("{0}&{1}",
                                 Uri.EscapeDataString(authOptions.ConsumerSecret),
                                 Uri.EscapeDataString(accessTokenSecret));
        }

        private static string GenerateNonce()
        {
            return Convert.ToBase64String(new ASCIIEncoding().GetBytes(DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture)));
        }

        private string CreateParameterString(SortedDictionary<string, string> sortedParams)
        {
            return string.Join("&", sortedParams.OrderBy(p => p.Key).Select(p => string.Format("{0}={1}", p.Key, p.Value)));
        }

        private string GenerateTimestamp()
        {
            var timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0);
            return Convert.ToInt64(timeSpan.TotalSeconds).ToString(CultureInfo.InvariantCulture);
        }
    }
}