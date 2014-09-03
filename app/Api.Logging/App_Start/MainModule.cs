using Autofac;
using Diagnostics;
using Logger = NLog.Logger;

namespace Api.Logging
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
        }
    }
}