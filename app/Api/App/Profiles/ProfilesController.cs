using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Api.App.Exceptions;
using Api.App.Profiles.Extensions;

namespace Api.App.Profiles
{
    [Authorize]
    [RoutePrefix("profiles")]
    public class ProfilesController : ApiBaseController
    {
        private readonly IProfilesManager profilesManager;
        private readonly string root = Path.Combine(Path.GetTempPath(), "uploads");

        public ProfilesController(IProfilesManager profilesManager)
        {
            this.profilesManager = profilesManager;
        }

        [Route("{userName}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(string userName)
        {
            try
            {
                var includePrivateData = User.Identity.Self(userName);
                var profile = await profilesManager.GetUserProfile(userName, includePrivateData);

                return Ok(profile);
            }
            catch (NotFoundException)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        //TODO: Think of another controller for such data
        [Route("countries")]
        [AllowAnonymous]
        public IHttpActionResult GetCountries()
        {
            return Ok(profilesManager.GetCountries());
        }

        [HttpGet]
        [Route("update-profile-image")]
        public async Task<IHttpActionResult> CheckProfileImagePart()
        {
            var flowChunkNumber = Int32.Parse(Request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "flowChunkNumber").Value);
            var flowIdentifier = Request.GetQueryNameValuePairs().FirstOrDefault(p => p.Key == "flowIdentifier").Value;

            if (ChunkIsHere(flowChunkNumber, flowIdentifier))
            {
                return Ok();
            }
            return ApiError(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Route("update-profile-image")]
        public async Task<IHttpActionResult> UpdateProfileImage()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            var provider = new MultipartFormDataStreamProvider(root);
            await Request.Content.ReadAsMultipartAsync(provider);
            var chunkNumber = Convert.ToInt32(provider.FormData["flowChunkNumber"]);
            var totalChunks = Convert.ToInt32(provider.FormData["flowTotalChunks"]);
            var identifier = provider.FormData["flowIdentifier"];
            var filename = provider.FormData["flowFilename"];
            // Rename generated file
            var chunk = provider.FileData[0]; // Only one file in multipart message
            RenameChunk(chunk, chunkNumber, identifier);
            // Assemble chunks into single file if they're all here
            TryAssembleFile(identifier, totalChunks, filename);
            // Success
            return Ok();
        }

        private void TryAssembleFile(string identifier, int totalChunks, string filename)
        {
            if (AllChunksAreHere(identifier, totalChunks))
            {
                // Create a single file
                var consolidatedFileName = GetFileName(identifier);
                using (var destStream = File.Create(consolidatedFileName, 15000))
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
                // Rename consolidated with original name of upload
                filename = Path.GetFileName(filename); // Strip to filename if directory is specified (avoid cross-directory attack)
                var realFileName = Path.Combine(root, filename);
                if (File.Exists(filename))
                {
                    File.Delete(realFileName);
                }
                File.Move(consolidatedFileName, realFileName);
                // Delete chunk files
                for (var chunkNumber = 1; chunkNumber <= totalChunks; chunkNumber++)
                {
                    var chunkFileName = GetChunkFileName(chunkNumber, identifier);
                    File.Delete(chunkFileName);
                }
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
            return Path.Combine(root, identifier);
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
            return Path.Combine(root, string.Format("{0}_{1}", identifier, chunkNumber.ToString()));
        }

        //private IEnumerable<UploadedFile> ContinuationFunction(MultipartFormDataStreamProvider streamProvider, string folderName, string rootUrl)
        //{
        //    {
        //        var fileInfo = streamProvider
        //            .FileData
        //            .Select(i =>
        //                    {
        //                        var info = new FileInfo(i.LocalFileName);
        //                        return new UploadedFile
        //                               {
        //                                   Name = info.Name,
        //                                   Path = rootUrl + "/" + folderName + "/" + info.Name,
        //                                   Size = info.Length / 1024
        //                               };
        //                    });
        //        return fileInfo;
        //    }
        //}
    }

    public class UploadedFile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
    }
}