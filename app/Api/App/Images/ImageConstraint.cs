using System.Drawing.Imaging;

namespace Api.App.Images
{
    public class ImageConstraint
    {
        public int MaxFileSizeMB { get; set; }
        public ImageFormat ImageFormat { get; set; }
        public SizeConstraints Size { get; set; }
    }

    public class SizeConstraints
    {
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
    }
}