namespace Api.App.Images.Exif
{
    public interface IExifReader
    {
        ExifData ReadExifData(string fileName);
    }
}