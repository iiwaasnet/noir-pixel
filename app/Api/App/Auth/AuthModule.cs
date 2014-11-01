using Autofac;

namespace Api.App.Auth
{
    public class AuthModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuthOptions>().AsSelf().SingleInstance();
        }
    }
}