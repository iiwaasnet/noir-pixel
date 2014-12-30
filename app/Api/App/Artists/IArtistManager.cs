using System.Threading.Tasks;

namespace Api.App.Artists
{
    public interface IArtistManager
    {
        Task<ArtistHome> GetUserHome(string userName);
    }
}