using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.App.Media
{
    public interface IMediaManager
    {
        bool MediaChunkReceived(HttpRequestMessage request, string userName);
        Task<MediaUploadResult> ReceiveMediaChunk(HttpRequestMessage request, string userName, Action<int> sizeAssert);
        MediaInfo SaveMediaFile(string fileName, string ownerId);
        MediaInfo SaveMediaUrl(string url, string ownerId);
        void DeleteMediaFile(string fileName);
        void DeleteMedia(string mediaId);
        MediaLink GetMediaLink(string mediaId);
    }
}