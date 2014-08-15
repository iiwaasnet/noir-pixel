namespace Web.Components
{
    public static class DeviceTypeExtensions
    {
        public static DeviceType ToDeviceType(this string deviceType)
        {
            switch (deviceType)
            {
                case "m":
                    return DeviceType.Mobile;
                case "t":
                    return DeviceType.Tablet;
                case "d":
                    return DeviceType.Desktop;
                default:
                    return DeviceType.Desktop;
            }
        }

        public static string ToDeviceType(this DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.Mobile:
                    return "m";
                case DeviceType.Tablet:
                    return "t";
                case DeviceType.Desktop:
                    return "d";
                default:
                    return null;
            }
        }
    }
}