using System.Net.Http;
using System.Threading.Tasks;

namespace Api.App.Media
{
    public interface IMediaUploadManager
    {
        bool MediaChunkReceived(HttpRequestMessage request, string userName);
        Task<MediaUploadResult> ReceiveMediaChunk(HttpRequestMessage request, string userName);
    }
}