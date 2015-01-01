using Api.App.Users.Entities;

namespace Api.App.Users
{
    public class UserHome
    {
        public string HomePageUrl { get; set; }
        public ProfileImage Thumbnail { get; set; }
        public string UserName { get; set; }
    }
}