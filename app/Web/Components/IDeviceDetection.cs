using System.Collections.Generic;
using System.Collections.Specialized;

namespace Web.Components
{
    public interface IDeviceDetection
    {
        DeviceType GetDeviceType(IDictionary<string, string> headers);
    }
}