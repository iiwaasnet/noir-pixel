using Autofac;

namespace Api.App.Users
{
    public class UsersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UsersesManager>()
                   .As<IUsersManager>()
                   .SingleInstance();
        }
    }
}