using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Api.App.Db;
using Api.App.Media.Config;
using Api.App.Profiles.Entities;
using JsonConfigurationProvider;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Api.App.Media
{
    public class MediaUploadManager : IMediaUploadManager
    {
        private readonly MediaConfiguration config;
        private readonly MongoDatabase db;

        public MediaUploadManager(IAppDbProvider dbProvider, IConfigProvider configProvider)
        {
            db = dbProvider.GetDatabase();
            config = configProvider.GetConfiguration<MediaConfiguration>();
        }

        public bool MediaChunkReceived(HttpRequestMessage request, string userName)
        {
            var flowChunkNumber = Int32.Parse(request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "flowChunkNumber").Value);
            var flowIdentifier = request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "flowIdentifier").Value;

            return ChunkIsHere(flowChunkNumber, flowIdentifier, GetUserId(userName));
        }

        public async Task<MediaUploadResult> ReceiveMediaChunk(HttpRequestMessage request, string userName)
        {
            AssertRequestIsMultipart(request);
            EnsureRootUploadFolderExists();

            var provider = await request.Content.ReadAsMultipartAsync(new MultipartFormDataStreamProvider(config.RootUploadFolder));
            var chunkInfo = GetChunkInfo(provider, userName);

            RenameChunk(chunkInfo);
            return TryAssembleFile(chunkInfo);
        }

        private string GetUserId(string userName)
        {
            var profiles = db.GetCollection<Profile>(Profile.CollectionName);
            var profile = profiles.FindOne(Query.EQ("UserName", userName));

            return profile.Id;
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
            if (AllChunksAreHere(chunkInfo.Identifier, chunkInfo.TotalChunks, chunkInfo.UserId))
            {
                var consolidatedFileName = GetFileName(chunkInfo.Identifier);

                ConcatFileParts(chunkInfo.Identifier, chunkInfo.TotalChunks, consolidatedFileName, chunkInfo.UserId);
                var fileName = RenameFinalFile(chunkInfo.FileName, consolidatedFileName, chunkInfo.UserId);
                DeleteChunks(chunkInfo.Identifier, chunkInfo.TotalChunks, chunkInfo.UserId);

                return new MediaUploadResult {Completed = true, FileName = fileName};
            }

            return new MediaUploadResult {Completed = false};
        }

        private void DeleteChunks(string identifier, int totalChunks, string userId)
        {
            for (var chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
            {
                var chunkFileName = GetChunkFileName(chunkNumber, identifier, userId);
                File.Delete(chunkFileName);
            }
        }

        private string RenameFinalFile(string fileName, string consolidatedFileName, string userId)
        {
            fileName = string.Format("{0}_{1}", userId, PreventCrossDirectoryAttack(fileName));
            var realFileName = Path.Combine(config.RootUploadFolder, fileName);

            if (File.Exists(realFileName))
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

        private void ConcatFileParts(string identifier, int totalChunks, string consolidatedFileName, string userId)
        {
            using (var destStream = File.Create(consolidatedFileName))
            {
                for (var chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
                {
                    var chunkFileName = GetChunkFileName(chunkNumber, identifier, userId);
                    using (var sourceStream = File.OpenRead(chunkFileName))
                    {
                        sourceStream.CopyTo(destStream);
                    }
                }
                destStream.Close();
            }
        }

        private bool AllChunksAreHere(string identifier, int totalChunks, string userId)
        {
            for (var chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
            {
                if (!ChunkIsHere(chunkNumber, identifier, userId))
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
            var chunkFileName = GetChunkFileName(chunkInfo.ChunkNumber, chunkInfo.Identifier, chunkInfo.UserId);
            if (File.Exists(chunkFileName))
            {
                File.Delete(chunkFileName);
            }
            File.Move(generatedFileName, chunkFileName);
        }

        private bool ChunkIsHere(int chunkNumber, string identifier, string userId)
        {
            var fileName = GetChunkFileName(chunkNumber, identifier, userId);
            return File.Exists(fileName);
        }

        private string GetChunkFileName(int chunkNumber, string identifier, string userId)
        {
            return Path.Combine(config.RootUploadFolder, string.Format("{0}_{1}_{2}", userId, identifier, chunkNumber));
        }

        private ChunkInfo GetChunkInfo(MultipartFormDataStreamProvider provider, string userName)
        {
            return new ChunkInfo
                   {
                       ChunkNumber = Convert.ToInt32(provider.FormData["flowChunkNumber"]),
                       TotalChunks = Convert.ToInt32(provider.FormData["flowTotalChunks"]),
                       Identifier = provider.FormData["flowIdentifier"],
                       FileName = provider.FormData["flowFilename"],
                       Chunk = provider.FileData[0],
                       UserId = GetUserId(userName)
                   };
        }
    }
}