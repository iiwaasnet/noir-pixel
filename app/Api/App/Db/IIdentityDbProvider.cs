using Api.App.Db.Config;

namespace Api.App.Db
{
    public interface IIdentityDbProvider : IDbProvider
    {
    }

    public class IdentityDbProvider : DbProvider, IIdentityDbProvider
    {
        public IdentityDbProvider(ConnectionConfiguration config) 
            : base(config)
        {
        }
    }
}