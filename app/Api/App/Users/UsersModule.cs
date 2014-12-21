using Autofac;

namespace Api.App.Users
{
    public class UsersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UsersManager>()
                   .As<IUserManager>()
                   .SingleInstance();
        }
    }
}