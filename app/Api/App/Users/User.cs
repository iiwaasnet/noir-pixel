using System;
using System.Collections.Generic;
using Api.App.Entities;
using MongoDB.Bson;

namespace Api.App.Users
{
    public class User : IEntity
    {
        public const string CollectionName = "users";

        public ObjectId _id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Geo LivesIn { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime DateRegistered { get; set; }
        public string AboutMe { get; set; }
        public IEnumerable<UserImage> UserImages { get; set; }
    }
}