using Autofac;

namespace Api.App.Media
{
    public class MediaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MediaManager>()
                   .As<IMediaManager>()
                   .SingleInstance();
            
            builder.RegisterType<ImageProcessor>()
                   .As<IImageProcessor>()
                   .SingleInstance();

            builder.RegisterType<MediaValidator>()
                   .As<IMediaValidator>()
                   .SingleInstance();
        }
    }
}