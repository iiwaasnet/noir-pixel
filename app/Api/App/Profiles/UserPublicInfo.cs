using System;
using System.Collections.Generic;

namespace Api.App.Profiles
{
    public class UserPublicInfo
    {
        public Geo LivesIn { get; set; }
        public DateTime DateRegistered { get; set; }
        public IEnumerable<string> AboutMe { get; set; }
        public string Avatar { get; set; }
        public string Thumbnail { get; set; }
    }
}