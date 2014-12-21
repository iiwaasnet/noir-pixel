using Api.App.Db.Config;

namespace Api.App.Db
{
    public interface IAppDbProvider : IDbProvider
    {
    }

    public class AppDbProvider : DbProvider, IAppDbProvider
    {
        public AppDbProvider(DbConfiguration config) 
            : base(config)
        {
        }
    }
}