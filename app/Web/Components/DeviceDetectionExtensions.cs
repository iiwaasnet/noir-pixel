using Microsoft.Owin.Extensions;
using Owin;
using Autofac.Integration.Owin;

namespace Web.Components
{
    public static class DeviceDetectionExtensions
    {
        public static IAppBuilder UseDeviceDetectionMiddleware(this IAppBuilder appBuilder)
        {
            //appBuilder.Use<DeviceDetectionMiddleware>();
            appBuilder.UseStageMarker(PipelineStage.PreHandlerExecute);

            return appBuilder;
        }
    }
}