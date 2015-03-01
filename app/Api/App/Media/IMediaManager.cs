using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api.App.Media
{
    public interface IMediaManager
    {
        bool MediaChunkReceived(HttpRequestMessage request, string userName);
        Task<MediaUploadResult> ReceiveMediaChunk(HttpRequestMessage request, string userName, IEnumerable<MediaConstraint> constraints);
        MediaInfo SaveMediaFile(string fileName, string ownerId);
        MediaInfo SaveMediaUrl(string url, string ownerId);
        void DeleteMediaFile(string fileName);
        void DeleteMedia(string mediaId);
        MediaLink GetMediaLink(string mediaId);
    }
}