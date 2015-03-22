using Api.App.Images.Exif;
using Autofac;

namespace Api.App.Images
{
    public class ImagesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProfileImageManager>()
                   .As<IProfileImageManager>()
                   .SingleInstance();
            
            builder.RegisterType<PhotosManager>()
                   .As<IPhotosManager>()
                   .SingleInstance();

            builder.RegisterType<ImageValidator>()
                   .As<IImageValidator>()
                   .SingleInstance();
            builder.RegisterType<ExifReader>()
                   .As<IExifReader>()
                   .SingleInstance();
        }
    }
}