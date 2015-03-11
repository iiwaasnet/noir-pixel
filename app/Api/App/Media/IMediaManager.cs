using System.Net.Http;
using System.Threading.Tasks;

namespace Api.App.Media
{
    public interface IMediaManager
    {
        Task<bool> MediaChunkReceived(HttpRequestMessage request, string userName);
        Task<MediaUploadResult> ReceiveMediaChunk(HttpRequestMessage request, string userName);
        Task<MediaInfo> SaveMediaFile(string fileName, string ownerId);
        Task<MediaInfo> SaveMediaUrl(string url, string ownerId);
        void DeleteMediaFile(string fileName);
        void DeleteMedia(string mediaId);
        Task<MediaLink> GetMediaLink(string mediaId);
    }
}