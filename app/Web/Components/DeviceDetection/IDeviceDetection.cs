using System.Collections.Generic;

namespace Web.Components.DeviceDetection
{
    public interface IDeviceDetection
    {
        DeviceType GetDeviceType(IDictionary<string, string> headers);
    }
}