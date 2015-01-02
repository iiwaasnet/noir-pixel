using Api.App.Users.Entities;

namespace Api.App.Users
{
    public class UserHome
    {
        public UserReference User { get; set; }
        public ProfileImage Thumbnail { get; set; }
    }
}