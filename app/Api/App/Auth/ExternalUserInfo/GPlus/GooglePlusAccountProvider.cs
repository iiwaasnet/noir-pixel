﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Api.App.Images.Config;
using TypedConfigProvider;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Owin.Security.Providers.GooglePlus;

namespace Api.App.Auth.ExternalUserInfo.GPlus
{
    public class GooglePlusAccountProvider : ISocialAccountProvider
    {
        private readonly GooglePlusAuthenticationOptions authOptions;
        private readonly ImagesConfiguration config;

        public GooglePlusAccountProvider(GooglePlusAuthenticationOptions authOptions,
                                         IConfigProvider configProvider)
        {
            this.authOptions = authOptions;
            config = configProvider.GetConfiguration<ImagesConfiguration>();
        }

        public async Task<ExternalUserInfo> GetUserInfo(string userId, string accessToken, string _)
        {
            var endPoint = string.Format("https://www.googleapis.com/plus/v1/people/{0}?access_token={1}", userId, accessToken);
            var client = new HttpClient();
            var uri = new Uri(endPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = JsonConvert.DeserializeObject<JObject>(content);
                if (string.Equals(userId, jObj.id.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var imageUrl = (string) jObj.image.url;

                    return new ExternalUserInfo
                           {
                               Person = new PersonInfo
                                        {
                                            Id = jObj.id,
                                            DisplayName = jObj.displayName,
                                            FirstName = jObj.name.givenName,
                                            LastName = jObj.name.familyName,
                                            Gender = jObj.gender,
                                            ThumbnailImage = imageUrl.ThumbnailUrl(config.ProfileImages.ThumbnailSize)
                                        },
                               Email = GetEmail(jObj.emails)
                           };
                }
            }

            //TODO: If response.IsSuccessStatusCode == false, then re-authentication needed
            return null;
        }

        public async Task<ParsedExternalAccessToken> VerifyAccessToken(string accessToken, string _)
        {
            var endPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}", accessToken);
            var client = new HttpClient();
            var uri = new Uri(endPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = JsonConvert.DeserializeObject<JObject>(content);
                if (string.Equals(authOptions.ClientId, jObj.audience.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return new ParsedExternalAccessToken
                           {
                               user_id = jObj.user_id,
                               app_id = jObj.audience,
                               email = jObj.email
                           };
                }
            }
            //TODO: If response.IsSuccessStatusCode == false, then re-authentication needed
            return null;
        }

        private static string GetEmail(IEnumerable<dynamic> emails)
        {
            var mail = emails.FirstOrDefault(m => m.type == "account") ?? emails.FirstOrDefault();

            return (mail != null) ? mail.value : null;
        }

        public string Provider
        {
            get { return authOptions.Caption; }
        }
    }
}