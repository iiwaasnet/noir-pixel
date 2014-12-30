using Autofac;

namespace Api.App.Artists
{
    public class ArtistsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ArtistsManager>()
                   .As<IArtistManager>()
                   .SingleInstance();
        }
    }
}