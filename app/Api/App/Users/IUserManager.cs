using System.Threading.Tasks;

namespace Api.App.Users
{
    public interface IUserManager
    {
        Task<UserHome> GetUserHome(string userName);
    }
}