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
        private readonly IMongoDatabase db;
        private readonly string mediaAccessRoute;

        public MediaManager(IAppDbProvider dbProvider, IConfigProvider configProvider)
        {
            mediaAccessRoute = RoutePrefix + "/{0}";
            db = dbProvider.GetDatabase();
            config = configProvider.GetConfiguration<MediaConfiguration>();
        }

        public async Task<bool> MediaChunkReceived(HttpRequestMessage request, string userName)
        {
            var flowChunkNumber = Int32.Parse(request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "flowChunkNumber").Value);
            var flowIdentifier = request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "flowIdentifier").Value;

            return ChunkIsHere(flowChunkNumber, flowIdentifier, await GetUserId(userName));
        }

        public async Task<MediaUploadResult> ReceiveMediaChunk(HttpRequestMessage request, string userName)
        {
            AssertRequestIsMultipart(request);
            EnsureRootUploadFolderExists();

            var provider = new MultipartFormDataStreamProvider(config.UploadFolder);
            provider = await request.Content.ReadAsMultipartAsync(provider);
            var chunkInfo = await GetChunkInfo(provider, userName);

            RenameChunk(chunkInfo);
            return TryAssembleFile(chunkInfo);
        }

        public async Task<MediaInfo> SaveMediaFile(string fileName, string ownerId)
        {
            var collection = db.GetCollection<Entities.Media>(Entities.Media.CollectionName);
            var media = new Entities.Media
                        {
                            OwnerId = ownerId,
                            Location = new MediaLocation {Location = fileName, Remote = false}
                        };
            media.Uri = GenerateMediaAccessUri(media.InitEntityId());

            await collection.InsertOneAsync(media);

            return new MediaInfo
                   {
                       MediaId = media.Id,
                       OwnerId = media.OwnerId,
                       Uri = media.Uri
                   };
        }

        public async Task<MediaInfo> SaveMediaUrl(string url, string ownerId)
        {
            var collection = db.GetCollection<Entities.Media>(Entities.Media.CollectionName);
            var media = new Entities.Media
                        {
                            OwnerId = ownerId,
                            Location = new MediaLocation {Location = url, Remote = true}
                        };
            media.Uri = GenerateMediaAccessUri(media.InitEntityId());

            await collection.InsertOneAsync(media);

            return new MediaInfo
                   {
                       MediaId = media.Id,
                       OwnerId = media.OwnerId,
                       Uri = media.Uri
                   };
        }

        public async Task DeleteMedia(string mediaId)
        {
            var collection = db.GetCollection<Entities.Media>(Entities.Media.CollectionName);
            var media = await collection.Find(m => m.Id == mediaId).FirstOrDefaultAsync();
            if (media != null)
            {
                if (!media.Location.Remote)
                {
                    DeleteMediaFile(media.Location.Location);
                }
                await collection.FindOneAndDeleteAsync(m => m.Id == media.Id);
            }
        }

        public void DeleteMediaFile(string fileName)
        {
            File.Delete(fileName);
        }

        public async Task<MediaLink> GetMediaLink(string mediaId)
        {
            var collection = db.GetCollection<Entities.Media>(Entities.Media.CollectionName);
            var media = await collection.Find(m => m.Id == mediaId).FirstOrDefaultAsync();
            if (media != null)
            {
                return new MediaLink
                       {
                           Location = media.Location.Location,
                           Remote = media.Location.Remote
                       };
            }

            return null;
        }

        private string GenerateMediaAccessUri(string id)
        {
            return string.Format(mediaAccessRoute, id);
        }

        private async Task<string> GetUserId(string userName)
        {
            return (await db.GetProfile(userName)).Id;
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
                var tmpConcatedFileName = GetFileName(chunkInfo.Identifier);

                ConcatFileParts(chunkInfo.Identifier, chunkInfo.TotalChunks, tmpConcatedFileName, chunkInfo.UserId);
                var fileName = RenameFinalFile(chunkInfo.FileName, tmpConcatedFileName, chunkInfo.UserId);
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

        private string RenameFinalFile(string fileName, string tmpFileName, string userId)
        {
            fileName = string.Format("{0}_{1}", userId, PreventCrossDirectoryAttack(fileName));
            var realFileName = Path.Combine(config.UploadFolder, fileName);

            if (File.Exists(realFileName))
            {
                File.Delete(realFileName);
            }
            File.Move(tmpFileName, realFileName);

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

        private async Task<ChunkInfo> GetChunkInfo(MultipartFormDataStreamProvider provider, string userName)
        {
            return new ChunkInfo
                   {
                       ChunkNumber = Convert.ToInt32(provider.FormData["flowChunkNumber"]),
                       TotalChunks = Convert.ToInt32(provider.FormData["flowTotalChunks"]),
                       Identifier = provider.FormData["flowIdentifier"],
                       FileName = provider.FormData["flowFilename"],
                       TotalSizeBytes = Convert.ToInt32(provider.FormData["flowTotalSize"]),
                       Chunk = provider.FileData[0],
                       UserId = await GetUserId(userName)
                   };
        }
    }
}