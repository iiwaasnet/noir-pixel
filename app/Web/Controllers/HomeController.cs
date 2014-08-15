using System.Web;
using System.Web.Mvc;
using Web.Components;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDeviceDetection deviceDetection;

        public HomeController(IDeviceDetection deviceDetection)
        {
            this.deviceDetection = deviceDetection;
        }

        public ActionResult Index()
        {
            var deviceType = GetDeviceType(Request);

            Response.Cookies.SetDeviceType(deviceType);
            return GetDeviceSpecificView(deviceType);
        }

        private ActionResult GetDeviceSpecificView(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.Mobile:
                    return View("Index.m");
                case DeviceType.Tablet:
                    return View("Index.t");
                case DeviceType.Desktop:
                    return View("Index");
                default:
                    return View("Index");
            }
        }

        private DeviceType GetDeviceType(HttpRequestBase request)
        {
            var deviceType = request.Cookies.GetDeviceType();

            return (deviceType != DeviceType.Undefined)
                       ? deviceType
                       : deviceDetection.GetDeviceType(request.Headers.ToDictionary());
        }
    }
}