namespace Api.App.Images
{
    public interface IPhotosManager
    {
        Photo SavePhoto(string userName, string fileName);
        void AssertFileSize(int fileSizeBytes);
        PendingPhotos GetPendingPhotos(string userName, int? offset, int? count);
    }
}