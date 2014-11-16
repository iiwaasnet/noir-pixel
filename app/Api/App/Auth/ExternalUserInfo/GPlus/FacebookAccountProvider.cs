using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;
using Newtonsoft.Json;
using Owin.Security.Providers.GooglePlus;

namespace Api.App.Auth.ExternalUserInfo.GPlus
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
            var endPoint = string.Format("https://www.googleapis.com/plus/v1/people/{0}?access_token={1}", userId, accessToken);
            var client = new HttpClient();
            var uri = new Uri(endPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = JsonConvert.DeserializeObject(content);
                if (string.Equals(userId, jObj.id.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return new ExternalUserInfo
                           {
                               Person = new PersonInfo
                                        {
                                            Id = jObj.id,
                                            DisplayName = jObj.displayName,
                                            FirstName = jObj.name.givenName,
                                            LastName = jObj.name.familyName,
                                            Gender = jObj.gender,
                                            Image = jObj.image.url
                                        },
                               Emails = GetEmails(jObj.emails)
                           };
                }
            }

            //TODO: If response.IsSuccessStatusCode == false, then re-authentication needed
            return null;
        }

        public async Task<ParsedExternalAccessToken> VerifyAccessToken(string accessToken)
        {
            var endPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            var client = new HttpClient();
            var uri = new Uri(endPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = JsonConvert.DeserializeObject(content);
                if (string.Equals(authOptions.AppId, jObj.app_id.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return new ParsedExternalAccessToken
                           {
                               user_id = jObj.user_id,
                               app_id = jObj.app_id,
                               email = jObj.email
                           };
                }
            }
            //TODO: If response.IsSuccessStatusCode == false, then re-authentication needed
            return null;
        }

        private static IEnumerable<EmailInfo> GetEmails(IEnumerable<dynamic> emails)
        {
            return emails.Select(m => new EmailInfo {Address = m.value, Type = m.type});
        }

        public string Provider
        {
            get { return "Facebook"; }
        }
    }
}