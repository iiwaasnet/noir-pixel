using System.Collections.Generic;

namespace Api.App.Auth.ExternalUserInfo
{
    public class ExternalUserInfo
    {
        public PersonInfo Person { get; set; }
        public IEnumerable<EmailInfo> Emails { get; set; }
        public IEnumerable<LinkInfo> Links { get; set; }
    }
}