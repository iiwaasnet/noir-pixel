namespace Api.App.Profiles
{
    public class UserProfile
    {
        public UserReference User { get; set; }
        public UserPublicInfo PublicInfo { get; set; }
        public UserPrivateInfo PrivateInfo { get; set; }
    }
}