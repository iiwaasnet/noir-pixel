using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Api.App.Images
{
    /// <summary>
    ///     Provides various image utilities, such as high quality resizing and the ability to save a JPEG.
    /// </summary>
    public static class ImageUtils
    {
        public static int QualityHighest = 100;
        private static readonly IDictionary<string, ImageCodecInfo> encoders;

        static ImageUtils()
        {
            encoders = ImageCodecInfo
                .GetImageEncoders()
                .ToDictionary(codec => codec.MimeType.ToLower(), codec => codec);
        }

        /// <summary>
        ///     Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            return DrawResizedImage(image, CalcNewSize(image, width, height));
        }

        public static Bitmap ResizeImageForCrop(Image image, int size)
        {
            return DrawResizedImage(image, CalcNewSize(image, size));
        }

        private static Size CalcNewSize(Image image, int width, int height)
        {
            var newWidth = image.Width;
            var newHeight = image.Height;
            var aspectRation = image.Width / (double) image.Height;
            var portrait = aspectRation <= 1;

            if (portrait && image.Height > height)
            {
                newHeight = height;
                newWidth = (int) Math.Floor(newHeight * aspectRation);
            }
            if (!portrait && image.Width > width)
            {
                newWidth = width;
                newHeight = (int) Math.Floor(newWidth / aspectRation);
            }

            return new Size {Height = newHeight, Width = newWidth};
        }

        private static Size CalcNewSize(Image image, int size)
        {
            var newWidth = image.Width;
            var newHeight = image.Height;
            var aspectRation = image.Width / (double) image.Height;
            var portrait = aspectRation <= 1;

            if (portrait && image.Width > size)
            {
                newWidth = size;
                newHeight = (int) Math.Floor(newWidth / aspectRation);
            }
            if (!portrait && image.Width > size)
            {
                newHeight = size;
                newWidth = (int) Math.Floor(newHeight * aspectRation);
            }

            return new Size {Height = newHeight, Width = newWidth};
        }

        private static Bitmap DrawResizedImage(Image image, Size newSize)
        {
            var result = new Bitmap(newSize.Width, newSize.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(image, new Rectangle {X = 0, Y = 0, Height = result.Height, Width = result.Width});
            }

            return result;
        }

        public static Bitmap CropImage(Image image, int width, int height)
        {
            var result = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphics.DrawImage(image,
                                   new Rectangle {X = 0, Y = 0, Height = result.Height, Width = result.Width},
                                   new Rectangle {X = (image.Width - result.Width) / 2, Y = (image.Height - result.Height) / 2, Height = result.Height, Width = result.Width},
                                   GraphicsUnit.Pixel);
            }

            return result;
        }

        public static void SaveJpeg(string path, Image image)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                SaveJpeg(stream, image, QualityHighest);
            }
        }

        /// <summary>
        ///     Saves an image as a jpeg image, with the given quality
        /// </summary>
        /// <param name="path">Path to which the image would be saved.</param>
        /// <param name="quality">
        ///     An integer from 0 to 100, with 100 being the
        ///     highest quality
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     An invalid value was entered for image quality.
        /// </exception>
        public static void SaveJpeg(string path, Image image, int quality)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                SaveJpeg(stream, image, quality);
            }
        }

        public static void SaveJpeg(Stream stream, Image image, int quality)
        {
            AssertJpegQualityValue(quality);

            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

            image.Save(stream, GetEncoderInfo("image/jpeg"), encoderParams);
        }

        private static void AssertJpegQualityValue(int quality)
        {
            if ((quality < 0) || (quality > 100))
            {
                throw new ArgumentOutOfRangeException(string.Format("Jpeg image quality must be between 0 and 100, with 100 being the highest quality.  A value of {0} was specified.", quality));
            }
        }

        /// <summary>
        ///     Returns the image codec with the given mime type
        /// </summary>
        public static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            var lookupKey = mimeType.ToLower();

            if (Encoders.ContainsKey(lookupKey))
            {
                return Encoders[lookupKey];
            }

            return null;
        }

        public static IDictionary<string, ImageCodecInfo> Encoders
        {
            get { return encoders; }
        }
    }
}