namespace Api.Core.Users
{
    public interface IUserManager
    {
        UserHome GetUserHome(string userName);
    }
}