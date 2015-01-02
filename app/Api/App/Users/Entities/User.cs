using System;
using System.Collections.Generic;
using Api.App.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Users.Entities
{
    public class User : Entity
    {
        public const string CollectionName = "users";

        public string UserId { get; set; }
        public string UserName { get; set; }

        [BsonIgnoreIfNull]
        public string FullName { get; set; }

        [BsonIgnoreIfNull]
        public string Email { get; set; }

        public Geo LivesIn { get; set; }
        public DateTime DateRegistered { get; set; }

        [BsonIgnoreIfNull]
        public IEnumerable<string> AboutMe { get; set; }

        public IEnumerable<ProfileImage> UserImages { get; set; }
    }
}