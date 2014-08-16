using Autofac;
using WURFL;
using WURFL.Aspnet.Extensions.Config;

namespace Web.Components.DeviceDetection
{
    public class DeviceDetectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => WURFLManagerBuilder.Build(new ApplicationConfigurer()))
                   .As<IWURFLManager>()
                   .SingleInstance();

            builder.RegisterType<DeviceDetection>().As<IDeviceDetection>().SingleInstance();
        }
    }
}