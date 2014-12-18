using Api.App.Entities;

namespace Api.App.Users
{
    public class UserImage : IEntity
    {
        public string _id { get; set; }
        public UserImageType ImageType { get; set; }
        public bool UserDefined { get; set; }
        public string Url { get; set; }
    }

    public enum UserImageType
    {
        Avatar,
        Thumbnail
    }
}