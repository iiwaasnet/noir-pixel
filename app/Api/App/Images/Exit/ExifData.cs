using System;

namespace Api.App.Images.Exit
{
    public class ExifData
    {
        public string Copyright { get; set; }
        public DateTime? DateTimeTaken { get; set; }
        public double? ExposureTime { get; set; }
        public ushort? Iso { get; set; }
        public double? FStop { get; set; }
        public double? FocalLength { get; set; }
        public string CameraMake { get; set; }
        public string CameraModel { get; set; }
        public string ShutterSpeed { get; set; }
    }
}