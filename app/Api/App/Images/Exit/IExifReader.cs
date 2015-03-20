namespace Api.App.Images.Exit
{
    public interface IExifReader
    {
        ExifData ReadExifData(string fileName);
    }
}