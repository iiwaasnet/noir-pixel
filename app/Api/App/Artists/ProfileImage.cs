using Api.App.Entities;

namespace Api.App.Artists
{
    public class ProfileImage : Entity
    {
        public ProfileImageType ImageType { get; set; }
        public bool UserDefined { get; set; }
        public string Url { get; set; }
    }

    public enum ProfileImageType
    {
        Avatar,
        Thumbnail
    }
}