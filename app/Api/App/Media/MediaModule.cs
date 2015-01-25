using Autofac;

namespace Api.App.Media
{
    public class MediaModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MediaUploadManager>()
                   .As<IMediaUploadManager>()
                   .SingleInstance();
        }
    }
}