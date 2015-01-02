using System;
using System.Collections.Generic;
using Api.App.Users;

namespace Api.App.Posts
{
    public class Reply
    {
        /// <summary>
        ///     In case this is a reply to a post, contains reference to it
        /// </summary>
        public string ReplyToPost { get; set; }

        /// <summary>
        ///     In case this is a replay to a discussion post, contains a reference to it
        /// </summary>
        public string ReplyToDiscussion { get; set; }

        public UserReference ReplyToUser { get; set; }
        public UserReference PostedBy { get; set; }

        /// <summary>
        ///     Array of ObjectIds of immediate replies to this reply
        /// </summary>
        public IEnumerable<string> Discussion { get; set; }

        public DateTime PostDate { get; set; }

        public string Content { get; set; }
    }
}