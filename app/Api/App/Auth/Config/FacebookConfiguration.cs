using System.Collections.Generic;

namespace Api.App.Auth.Config
{
    public class FacebookConfiguration
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public IEnumerable<string> UserScope { get; set; }
    }
}