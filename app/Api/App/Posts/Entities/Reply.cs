using System;
using System.Collections.Generic;
using Api.App.Entities;
using Api.App.Profiles.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Posts.Entities
{
    public class Reply : Entity
    {
        /// <summary>
        /// In case this is a reply to a post, contains reference to it
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ReplyToPost { get; set; }
        /// <summary>
        /// In case this is a replay to a discussion post, contains a reference to it
        /// </summary>
        [BsonIgnoreIfNull]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ReplyToDiscussion { get; set; }

        public UserReference ReplyToUser { get; set; }
        public UserReference PostedBy { get; set; }
        /// <summary>
        /// Array of ObjectIds of immediate replies to this reply
        /// </summary>
        [BsonIgnoreIfNull]
        public IEnumerable<ObjectId> Discussion { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime PostDate { get; set; }

        public string Content { get; set; }
    }
}