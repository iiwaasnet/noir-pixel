using System.Net.Http;
using System.Threading.Tasks;

namespace Api.App.Media
{
    public interface IMediaUploadManager
    {
        bool MediaChunkReceived(HttpRequestMessage request);
        Task<string> ReceiveMediaChunk(HttpRequestMessage request);
    }
}