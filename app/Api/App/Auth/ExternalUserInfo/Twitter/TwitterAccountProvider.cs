using System;
using System.Collections.Generic;
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
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", authHeader);
            var uri = new Uri(endPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = JsonConvert.DeserializeObject<JObject>(content);
                if (string.Equals(userId, jObj.id.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return new ExternalUserInfo
                           {
                               Person = new PersonInfo
                                        {
                                            Id = jObj.id,
                                            DisplayName = jObj.name,
                                            FirstName = jObj.first_name,
                                            LastName = jObj.last_name,
                                            Gender = jObj.gender,
                                            Image = string.Format("https://graph.facebook.com/me?access_token={0}", accessToken)
                                        },
                               Email = jObj.email
                           };
                }
            }
            //TODO: If response.IsSuccessStatusCode == false, then re-authentication needed
            return null;
        }

        public async Task<ParsedExternalAccessToken> VerifyAccessToken(string accessToken, string accessTokenSecret)
        {
            var userId = "iiwaasnet";
            var baseUrl = "https://api.twitter.com/1.1/users/show.json";
            var queryString = new Dictionary<string, string> { { "screen_name", userId } };
            var requestAuth = new AuthorizeRequest(authOptions);
            var authHeader = requestAuth.CreateAuthHeader("GET", baseUrl, accessToken, accessTokenSecret, queryString);

            var endPoint = string.Format("{0}?screen_name={1}", baseUrl, userId);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", authHeader);
            var uri = new Uri(endPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = JsonConvert.DeserializeObject<JObject>(content);
                if (string.Equals(authOptions.ConsumerKey, jObj.audience.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return new ParsedExternalAccessToken
                    {
                        user_id = jObj.id_str,
                        app_id = jObj.audience,
                        email = jObj.email
                    };
                }
            }
            //TODO: If response.IsSuccessStatusCode == false, then re-authentication needed
            return null;
        }

        private string GetAccessToken()
        {
            return "";
            //return string.Format("{0}|{1}", authOptions.AppId, authOptions.AppSecret);
        }

        public string Provider
        {
            get { return authOptions.Caption; }
        }
    }
}