using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Api.App.Db;
using Api.App.Db.Extensions;
using Api.App.Media.Config;
using Api.App.Media.Entities;
using JsonConfigurationProvider;
using MongoDB.Driver;

namespace Api.App.Media
{
    public class MediaManager : IMediaManager
    {
        public const string RoutePrefix = "media";

        private readonly MediaConfiguration config;
        private readonly MongoDatabase db;
        private readonly string mediaAccessRoute;

        public MediaManager(IAppDbProvider dbProvider, IConfigProvider configProvider)
        {
            mediaAccessRoute = RoutePrefix + "/{0}";
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

            var provider = await request.Content.ReadAsMultipartAsync(new MultipartFormDataStreamProvider(config.UploadFolder));
            var chunkInfo = GetChunkInfo(provider, userName);

            RenameChunk(chunkInfo);
            return TryAssembleFile(chunkInfo);
        }

        public MediaInfo SaveMedia(string fileName, string ownerId)
        {
            var collection = db.GetCollection<Entities.Media>(Entities.Media.CollectionName);
            var media = new Entities.Media
                        {
                            OwnerId = ownerId,
                            Location = new MediaLocation {LocalPath = fileName}
                        };
            media.Uri = GenerateMediaAccessUri(media.Id);

            collection.Insert(media);

            return new MediaInfo
                   {
                       MediaId = media.Id,
                       OwnerId = media.OwnerId,
                       Uri = media.Uri
                   };
        }

        public void DeleteMedia(string fileName)
        {
            File.Delete(fileName);
        }

        private string GenerateMediaAccessUri(string id)
        {
            return string.Format(mediaAccessRoute, id);
        }

        private string GetUserId(string userName)
        {
            return db.GetProfile(userName).Id;
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
            if (!Directory.Exists(config.UploadFolder))
            {
                Directory.CreateDirectory(config.UploadFolder);
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
            var realFileName = Path.Combine(config.UploadFolder, fileName);

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
            return Path.Combine(config.UploadFolder, identifier);
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
            return Path.Combine(config.UploadFolder, string.Format("{0}_{1}_{2}", userId, identifier, chunkNumber));
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