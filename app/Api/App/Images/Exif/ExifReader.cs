using System;
using System.Globalization;
using Api.App.Framework;
using Diagnostics;
using ExifLib;

namespace Api.App.Images.Exif
{
    public class ExifReader : IExifReader
    {
        private readonly ILogger logger;

        public ExifReader(ILogger logger)
        {
            this.logger = logger;
        }

        public ExifData ReadExifData(string fileName)
        {
            var exifRetrieved = false;
            var exifData = new ExifData();
            using (var exifReader = new ExifLib.ExifReader(fileName))
            {
                exifRetrieved = (exifData.ShutterSpeed = SafeReadExifTag(ReadShutterSpeed, exifReader)) != null;
                exifRetrieved = exifRetrieved | (exifData.Iso = SafeReadExifTag(ReadIso, exifReader)) != null;
                exifRetrieved = exifRetrieved | (exifData.FocalLength = SafeReadExifTag(ReadFocalLength, exifReader)) != null;
                exifRetrieved = exifRetrieved | (exifData.FStop = SafeReadExifTag(ReadFStop, exifReader)) != null;
                exifRetrieved = exifRetrieved | (exifData.ExposureTime = SafeReadExifTag(ReadExposureTime, exifReader)) != null;
                exifRetrieved = exifRetrieved | (exifData.CameraModel = SafeReadExifTag(ReadCameraModel, exifReader)) != null;
                exifRetrieved = exifRetrieved | (exifData.Copyright = SafeReadExifTag(ReadCopyright, exifReader)) != null;
                exifRetrieved = exifRetrieved | (exifData.DateTimeTaken = SafeReadExifTag(ReadDateTaken, exifReader)) != null;
            }

            return exifRetrieved ? exifData : null;
        }

        private DateTime? ReadDateTaken(ExifLib.ExifReader exifReader)
        {
            string value;
            exifReader.GetTagValue(ExifTags.DateTime, out value);
            DateTime dateTime;

            if (!string.IsNullOrWhiteSpace(value)
                && DateTime.TryParseExact(value, "yyyy':'MM':'dd HH':'mm':'ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }

            return null;
        }

        private string ReadCopyright(ExifLib.ExifReader exifReader)
        {
            string value;
            return exifReader.GetTagValue(ExifTags.Copyright, out value)
                       ? value
                       : null;
        }

        private string ReadCameraModel(ExifLib.ExifReader exifReader)
        {
            string value;
            return exifReader.GetTagValue(ExifTags.Model, out value)
                       ? value
                       : null;
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

        private T SafeReadExifTag<T>(Antlr.Runtime.Misc.Func<ExifLib.ExifReader, T> readExifTag, ExifLib.ExifReader exifReader)
        {
            try
            {
                return readExifTag(exifReader);
            }
            catch (Exception err)
            {
                logger.Error(err);
            }

            return default (T);
        }
    }
}