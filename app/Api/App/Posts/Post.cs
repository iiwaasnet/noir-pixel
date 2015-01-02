using System;
using System.Collections.Generic;
using Api.App.Users;

namespace Api.App.Posts
{
    public class Post
    {
        public UserReference PostedBy { get; set; }
        public DateTime PostDate { get; set; }
        public string Content { get; set; }
        public int RepliesCount { get; set; }
        public IEnumerable<Reply> Discussion { get; set; }
    }
}