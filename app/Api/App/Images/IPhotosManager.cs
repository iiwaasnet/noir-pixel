﻿using System.Threading.Tasks;

namespace Api.App.Images
{
    public interface IPhotosManager
    {
        Task<Photo> SavePhoto(string userName, string fileName);
        PendingPhotos GetPendingPhotos(string userName, int? offset, int? count);
    }
}