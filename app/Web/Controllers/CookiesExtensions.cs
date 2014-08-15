using System;
using System.Web;
using Web.Components;

namespace Web.Controllers
{
    public static class CookiesExtensions
    {
        private const string DeviceCookie = "device";

        public static DeviceType GetDeviceType(this HttpCookieCollection cookies)
        {
            var cookie = cookies[DeviceCookie];
            if (cookie != null)
            {
                return cookie.Value.ToDeviceType();
            }

            return DeviceType.Undefined;
        }

        public static void SetDeviceType(this HttpCookieCollection cookies, DeviceType deviceType)
        {
            cookies.Add(new HttpCookie(DeviceCookie, deviceType.ToDeviceType())
                        {
                            Expires = DateTime.MaxValue
                        });
        }
    }
}