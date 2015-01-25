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

        public async Task<string> ReceiveMediaChunk(HttpRequestMessage request)
        {
            AssertRequestIsMultipart(request);
            EnsureRootUploadFolderExists();
            var provider = await request.Content.ReadAsMultipartAsync(new MultipartFormDataStreamProvider(config.RootUploadFolder));

            var chunkNumber = Convert.ToInt32(provider.FormData["flowChunkNumber"]);
            var totalChunks = Convert.ToInt32(provider.FormData["flowTotalChunks"]);
            var identifier = provider.FormData["flowIdentifier"];
            var filename = provider.FormData["flowFilename"];
            var chunk = provider.FileData[0];

            RenameChunk(chunk, chunkNumber, identifier);

            TryAssembleFile(identifier, totalChunks, filename);

            return "";
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

        private void TryAssembleFile(string identifier, int totalChunks, string filename)
        {
            if (AllChunksAreHere(identifier, totalChunks))
            {
                var consolidatedFileName = GetFileName(identifier);

                ConcatFileParts(identifier, totalChunks, consolidatedFileName);
                RenameFinalFile(filename, consolidatedFileName);
                DeleteChunks(identifier, totalChunks);
            }
        }

        private void DeleteChunks(string identifier, int totalChunks)
        {
            for (var chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
            {
                var chunkFileName = GetChunkFileName(chunkNumber, identifier);
                File.Delete(chunkFileName);
            }
        }

        private void RenameFinalFile(string filename, string consolidatedFileName)
        {
            filename = Path.GetFileName(filename); // Strip to filename if directory is specified (avoid cross-directory attack)
            var realFileName = Path.Combine(config.RootUploadFolder, filename);
            if (File.Exists(filename))
            {
                File.Delete(realFileName);
            }
            File.Move(consolidatedFileName, realFileName);
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

        private void RenameChunk(MultipartFileData chunk, int chunkNumber, string identifier)
        {
            var generatedFileName = chunk.LocalFileName;
            var chunkFileName = GetChunkFileName(chunkNumber, identifier);
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
    }
}