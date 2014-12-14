namespace Api.App.Users
{
    public interface IUserManager
    {
        UserHome GetUserHome(string userName);
    }
}