using System;
using System.Collections.Generic;
using Api.App.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Users.Entities
{
    public class Post : Entity
    {
        public PostTargetReference PostTarget { get; set; }
        public UserReference PostedBy { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime PostDate { get; set; }

        public string Content { get; set; }
        public IEnumerable<DiscussionPost> Discussion { get; set; }
    }
}