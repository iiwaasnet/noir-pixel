﻿using System;
using System.Collections.Generic;
using Api.App.Entities;
using Api.App.Images.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.App.Profiles.Entities
{
    public class Profile : Entity
    {
        public const string CollectionName = "profiles";

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonRequired]
        public string UserId { get; set; }
        [BsonRequired]
        public string UserName { get; set; }

        [BsonIgnoreIfNull]
        public string FullName { get; set; }

        [BsonIgnoreIfNull]
        public string Email { get; set; }

        [BsonIgnoreIfNull]
        public Geo LivesIn { get; set; }
        public DateTime DateRegistered { get; set; }

        [BsonIgnoreIfNull]
        public IEnumerable<string> AboutMe { get; set; }

        public ProfileImage UserImage { get; set; }
    }
}