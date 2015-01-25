using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Api.App.Media.Config;
using JsonConfigurationProvider;

namespace Api.App.Media
{
    public class MediaUploadManager : IMediaUploadManager
    {
        private readonly MediaConfiguration config;

        public MediaUploadManager(IConfigProvider configProvider)
        {
            config = configProvider.GetConfiguration<MediaConfiguration>();
        }

        public bool MediaChunkReceived(HttpRequestMessage request)
        {
            var flowChunkNumber = Int32.Parse(request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "flowChunkNumber").Value);
            var flowIdentifier = request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "flowIdentifier").Value;

            return ChunkIsHere(flowChunkNumber, flowIdentifier);
        }

        public async Task<MediaUploadResult> ReceiveMediaChunk(HttpRequestMessage request)
        {
            AssertRequestIsMultipart(request);
            EnsureRootUploadFolderExists();

            var provider = await request.Content.ReadAsMultipartAsync(new MultipartFormDataStreamProvider(config.RootUploadFolder));
            var chunkInfo = GetChunkInfo(provider);

            RenameChunk(chunkInfo);
            return TryAssembleFile(chunkInfo);
        }

        private static void AssertRequestIsMultipart(HttpRequestMessage request)
        {
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new NotSupportedException();
            }
        }

        private void EnsureRootUploadFolderExists()
        {
            if (!Directory.Exists(config.RootUploadFolder))
            {
                Directory.CreateDirectory(config.RootUploadFolder);
            }
        }

        private MediaUploadResult TryAssembleFile(ChunkInfo chunkInfo)
        {
            if (AllChunksAreHere(chunkInfo.Identifier, chunkInfo.TotalChunks))
            {
                var consolidatedFileName = GetFileName(chunkInfo.Identifier);

                ConcatFileParts(chunkInfo.Identifier, chunkInfo.TotalChunks, consolidatedFileName);
                var fileName = RenameFinalFile(chunkInfo.FileName, consolidatedFileName);
                DeleteChunks(chunkInfo.Identifier, chunkInfo.TotalChunks);

                return new MediaUploadResult {Completed = true, FileName = fileName};
            }

            return new MediaUploadResult {Completed = false};
        }

        private void DeleteChunks(string identifier, int totalChunks)
        {
            for (var chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
            {
                var chunkFileName = GetChunkFileName(chunkNumber, identifier);
                File.Delete(chunkFileName);
            }
        }

        private string RenameFinalFile(string filename, string consolidatedFileName)
        {
            var realFileName = Path.Combine(config.RootUploadFolder, PreventCrossDirectoryAttack(filename));
            if (File.Exists(filename))
            {
                File.Delete(realFileName);
            }
            File.Move(consolidatedFileName, realFileName);

            return realFileName;
        }

        private static string PreventCrossDirectoryAttack(string filename)
        {
            return Path.GetFileName(filename);
        }

        private void ConcatFileParts(string identifier, int totalChunks, string consolidatedFileName)
        {
            using (var destStream = File.Create(consolidatedFileName))
            {
                for (var chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
                {
                    var chunkFileName = GetChunkFileName(chunkNumber, identifier);
                    using (var sourceStream = File.OpenRead(chunkFileName))
                    {
                        sourceStream.CopyTo(destStream);
                    }
                }
                destStream.Close();
            }
        }

        private bool AllChunksAreHere(string identifier, int totalChunks)
        {
            for (var chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
            {
                if (!ChunkIsHere(chunkNumber, identifier))
                {
                    return false;
                }
            }
            return true;
        }

        private string GetFileName(string identifier)
        {
            return Path.Combine(config.RootUploadFolder, identifier);
        }

        private void RenameChunk(ChunkInfo chunkInfo)
        {
            var generatedFileName = chunkInfo.Chunk.LocalFileName;
            var chunkFileName = GetChunkFileName(chunkInfo.ChunkNumber, chunkInfo.Identifier);
            if (File.Exists(chunkFileName))
            {
                File.Delete(chunkFileName);
            }
            File.Move(generatedFileName, chunkFileName);
        }

        private bool ChunkIsHere(int chunkNumber, string identifier)
        {
            var fileName = GetChunkFileName(chunkNumber, identifier);
            return File.Exists(fileName);
        }

        private string GetChunkFileName(int chunkNumber, string identifier)
        {
            return Path.Combine(config.RootUploadFolder, string.Format("{0}_{1}", identifier, chunkNumber));
        }

        private static ChunkInfo GetChunkInfo(MultipartFormDataStreamProvider provider)
        {
            return new ChunkInfo
                   {
                       ChunkNumber = Convert.ToInt32(provider.FormData["flowChunkNumber"]),
                       TotalChunks = Convert.ToInt32(provider.FormData["flowTotalChunks"]),
                       Identifier = provider.FormData["flowIdentifier"],
                       FileName = provider.FormData["flowFilename"],
                       Chunk = provider.FileData[0]
                   };
        }
    }
}