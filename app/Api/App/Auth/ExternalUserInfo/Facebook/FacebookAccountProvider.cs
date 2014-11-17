using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Api.App.Auth.ExternalUserInfo.Facebook
{
    public class FacebookAccountProvider : ISocialAccountProvider
    {
        private readonly FacebookAuthenticationOptions authOptions;

        public FacebookAccountProvider(FacebookAuthenticationOptions authOptions)
        {
            this.authOptions = authOptions;
        }

        public async Task<ExternalUserInfo> GetUserInfo(string userId, string accessToken)
        {
            var endPoint = string.Format("https://graph.facebook.com/me?access_token={0}", accessToken);
            var client = new HttpClient();
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

        public async Task<ParsedExternalAccessToken> VerifyAccessToken(string accessToken)
        {
            var endPoint = string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken, GetAccessToken());
            var client = new HttpClient();
            var uri = new Uri(endPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = JsonConvert.DeserializeObject<JObject>(content);
                var appId = jObj.data.app_id;
                if (string.Equals(authOptions.AppId, appId.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return new ParsedExternalAccessToken
                           {
                               user_id = jObj.data.user_id,
                               app_id = appId
                           };
                }
            }
            //TODO: If response.IsSuccessStatusCode == false, then re-authentication needed
            return null;
        }

        private string GetAccessToken()
        {
            return string.Format("{0}|{1}", authOptions.AppId, authOptions.AppSecret);
        }

        public string Provider
        {
            get { return authOptions.Caption; }
        }
    }
}