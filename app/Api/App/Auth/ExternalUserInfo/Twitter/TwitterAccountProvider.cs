using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Twitter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Api.App.Auth.ExternalUserInfo.Twitter
{
    public class TwitterAccountProvider : ISocialAccountProvider
    {
        private readonly TwitterAuthenticationOptions authOptions;

        public TwitterAccountProvider(TwitterAuthenticationOptions authOptions)
        {
            this.authOptions = authOptions;
        }

        public async Task<ExternalUserInfo> GetUserInfo(string userId, string accessToken, string accessTokenSecret)
        {
            var baseUrl = "https://api.twitter.com/1.1/users/show.json";
            var queryString = new Dictionary<string, string> {{"user_id", userId}};
            var requestAuth = new AuthorizeRequest(authOptions);
            var authHeader = requestAuth.CreateAuthHeader("GET", baseUrl, accessToken, accessTokenSecret, queryString);

            var endPoint = string.Format("{0}?user_id={1}", baseUrl, userId);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", authHeader);
            var uri = new Uri(endPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = JsonConvert.DeserializeObject<JObject>(content);
                if (string.Equals(userId, jObj.id_str.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return new ExternalUserInfo
                           {
                               Person = new PersonInfo
                                        {
                                            Id = jObj.id_str,
                                            DisplayName = jObj.screen_name,
                                            FullName = jObj.name,
                                            ThumbnailImage = jObj.profile_image_url
                                        }
                           };
                }
            }
            //TODO: If response.IsSuccessStatusCode == false, then re-authentication needed
            return null;
        }

        public async Task<ParsedExternalAccessToken> VerifyAccessToken(string accessToken, string accessTokenSecret)
        {
            var endPoint = "https://api.twitter.com/1.1/account/verify_credentials.json";
            var requestAuth = new AuthorizeRequest(authOptions);
            var authHeader = requestAuth.CreateAuthHeader("GET", endPoint, accessToken, accessTokenSecret, EmptyQueryString());

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", authHeader);
            var uri = new Uri(endPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                dynamic jObj = JsonConvert.DeserializeObject<JObject>(content);

                return new ParsedExternalAccessToken {user_id = jObj.id_str};
            }
            //TODO: If response.IsSuccessStatusCode == false, then re-authentication needed
            return null;
        }

        private static IEnumerable<KeyValuePair<string, string>> EmptyQueryString()
        {
            return Enumerable.Empty<KeyValuePair<string, string>>();
        }

        public string Provider
        {
            get { return authOptions.Caption; }
        }
    }
}