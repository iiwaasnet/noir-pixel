using System;
using System.Collections.Generic;
using Api.App.Entities;
using Api.App.Users.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Posts.Entities
{
    public class Post : Entity
    {
        public const string CollectionName = "posts";

        public PostTargetReference PostTarget { get; set; }
        public UserReference PostedBy { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime PostDate { get; set; }

        public string Content { get; set; }
        public int RepliesCount { get; set; }

        /// <summary>
        ///     All discussion posts done to this post
        /// </summary>
        [BsonIgnoreIfNull]
        public IEnumerable<Reply> Discussion { get; set; }
    }
}