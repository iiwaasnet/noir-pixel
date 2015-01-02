using System;
using System.Collections.Generic;
using Api.App.Users.Entities;

namespace Api.App.Users
{
    public class UserPublicInfo
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public Geo LivesIn { get; set; }
        public DateTime DateRegistered { get; set; }
        public IEnumerable<string> AboutMe { get; set; }
        public ProfileImage Avatar { get; set; }
        public ProfileImage Thumbnail { get; set; }
    }
}