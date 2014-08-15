using System.Collections.Generic;
using WURFL;
using WURFL.Request;

namespace Web.Components
{
    public class DeviceDetection : IDeviceDetection
    {
        private readonly IWURFLManager wurflManager;

        public DeviceDetection(IWURFLManager wurflManager)
        {
            this.wurflManager = wurflManager;
        }

        public DeviceType GetDeviceType(IDictionary<string, string> headers)
        {
            var deviceInfo = wurflManager.GetDeviceForRequest(new WURFLRequest(headers));

            if (deviceInfo.GetVirtualCapability("is_smartphone") == "true")
            {
                return DeviceType.Mobile;
            }
            if (deviceInfo.GetVirtualCapability("is_mobile") == "true")
            {
                return DeviceType.Tablet;
            }

            return DeviceType.Desktop;
        }
    }
}