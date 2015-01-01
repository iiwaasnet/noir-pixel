using System;
using Api.App.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Users.Entities
{
    public class DiscussionPost : Entity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ReplyToPost { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ReplyToDiscussion { get; set; }

        public UserReference ReplyToUser { get; set; }
        public UserReference PostedBy { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime PostDate { get; set; }

        private string Content { get; set; }
    }
}