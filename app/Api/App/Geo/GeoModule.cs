using Autofac;

namespace Api.App.Geo
{
    public class GeoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GeoManager>()
                   .As<IGeoManager>()
                   .SingleInstance();
        }
    }
}