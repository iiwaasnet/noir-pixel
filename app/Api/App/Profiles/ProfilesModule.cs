using Autofac;

namespace Api.App.Profiles
{
    public class ProfilesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProfilesManager>()
                   .As<IProfilesManager>()
                   .SingleInstance();
        }
    }
}