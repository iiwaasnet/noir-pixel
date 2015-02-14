namespace Api.App.Images
{
    public interface IPhotosManager
    {
        Photo SavePhoto(string userName, string fileName);
    }
}