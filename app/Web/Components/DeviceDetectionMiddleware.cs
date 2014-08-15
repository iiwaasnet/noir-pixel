using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.Owin;
using Microsoft.Owin;
using WURFL;
using WURFL.Request;

namespace Web.Components
{
    //public class DeviceDetectionMiddleware : OwinMiddleware
    //{
    //    private IWURFLManager wurflManager;

    //    public DeviceDetectionMiddleware(OwinMiddleware next)
    //        : base(next)
    //    {
    //    }

    //    public override async Task Invoke(IOwinContext context)
    //    {
    //        var deviceType = DeviceType.Undefined;

    //        var cookie = context.Request.Cookies["device"];
    //        if (string.IsNullOrWhiteSpace(cookie))
    //        {
    //            wurflManager = context.GetAutofacLifetimeScope().Resolve<IWURFLManager>();

    //            var httpHeaders = context.Request.Headers.ToHttpHeaders();
    //            var deviceInfo = wurflManager.GetDeviceForRequest(new WURFLRequest(httpHeaders));


    //            if (deviceInfo.GetVirtualCapability("is_smartphone") == "true")
    //            {
    //                deviceType = DeviceType.Mobile;
    //            }
    //        }
    //        if (cookie == "m")
    //        {
    //            deviceType = DeviceType.Mobile;
    //        }

    //        if (deviceType == DeviceType.Mobile)
    //        {
    //            context.Response.Cookies.Append("device", "m");
    //            context.Response.Redirect("http://m.noir-pixel.com");
    //        }
    //        else
    //        {
    //            await Next.Invoke(context);
    //        }
    //    }
    //}
}