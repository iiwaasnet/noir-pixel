using Api.App.Framework;
using ExifLib;

namespace Api.App.Images.Exit
{
    public class ExifReader : IExifReader
    {
        public ExifData ReadExifData(string fileName)
        {
            var exifRetrieved = false;
            var exifData = new ExifData();
            using (var exifReader = new ExifLib.ExifReader(fileName))
            {
                exifRetrieved = (exifData.ShutterSpeed = ReadShutterSpeed(exifReader)) != null;
                exifRetrieved = exifRetrieved | (exifData.Iso = ReadIso(exifReader)) != null;
                exifRetrieved = exifRetrieved | (exifData.FocalLength = ReadFocalLength(exifReader)) != null;
                exifRetrieved = exifRetrieved | (exifData.FStop = ReadFStop(exifReader)) != null;
                exifRetrieved = exifRetrieved | (exifData.ExposureTime = ReadExposureTime(exifReader)) != null;
            }

            return exifRetrieved ? exifData : null;
        }

        private double? ReadExposureTime(ExifLib.ExifReader exifReader)
        {
            double value;
            return exifReader.GetTagValue(ExifTags.ExposureTime, out value)
                       ? value
                       : (double?) null;
        }

        private double? ReadFStop(ExifLib.ExifReader exifReader)
        {
            double value;
            return exifReader.GetTagValue(ExifTags.FNumber, out value)
                       ? value
                       : (double?) null;
        }

        private double? ReadFocalLength(ExifLib.ExifReader exifReader)
        {
            double value;
            return exifReader.GetTagValue(ExifTags.FocalLength, out value)
                       ? value
                       : (double?) null;
        }

        private ushort? ReadIso(ExifLib.ExifReader exifReader)
        {
            ushort value;
            return exifReader.GetTagValue(ExifTags.ISOSpeedRatings, out value)
                       ? value
                       : (ushort?) null;
        }

        private string ReadShutterSpeed(ExifLib.ExifReader exifReader)
        {
            double value;
            return exifReader.GetTagValue(ExifTags.ExposureTime, out value)
                       ? new Fraction(value).ToString()
                       : null;
        }
    }
}