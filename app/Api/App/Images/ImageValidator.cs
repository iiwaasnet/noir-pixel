using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Api.App.Exceptions;

namespace Api.App.Images
{
    public class ImageValidator : IImageValidator
    {
        public void Assert(string fileName, IEnumerable<ImageConstraint> constraints)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var image = LoadImage(stream))
                {
                    var constraint = AssertImageType(constraints, image);
                    AssertFileSize(stream, constraint);
                    AssertImageSize(image, constraint);
                }
            }
        }

        private static Image LoadImage(FileStream stream)
        {
            try
            {
                return Image.FromStream(stream, false, false);
            }
            catch
            {
                throw new UnsupportedImageFormatException();
            }
        }

        private static void AssertImageSize(Image image, ImageConstraint constraint)
        {
            if (image.Height < constraint.Size.MinHeight
                || image.Height > constraint.Size.MaxHeight
                || image.Width < constraint.Size.MinWidth
                || image.Width > constraint.Size.MaxWidth)
            {
                throw new ImageSizeConstraintsException();
            }
        }

        private static void AssertFileSize(FileStream stream, ImageConstraint constraint)
        {
            var fileSizeMB = (int) stream.Length / 1024 / 1024;
            if (fileSizeMB > constraint.MaxFileSizeMB)
            {
                throw new OverMaxAllowedFileSizeException(constraint.MaxFileSizeMB, fileSizeMB);
            }
        }

        private static ImageConstraint AssertImageType(IEnumerable<ImageConstraint> constraints, Image image)
        {
            var constraint = constraints.FirstOrDefault(c => c.ImageFormat.Equals(image.RawFormat));
            if (constraint == null)
            {
                throw new UnsupportedImageFormatException();
            }

            return constraint;
        }
    }
}