using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Api.App.Exceptions;
using Api.App.Media;
using Api.App.Profiles.Extensions;

namespace Api.App.Profiles
{
    [Authorize]
    [RoutePrefix("profiles")]
    public class ProfilesController : ApiBaseController
    {
        private readonly IProfilesManager profilesManager;

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

        [HttpPost]
        [HttpGet]
        [Route("update-profile-image")]
        public async Task<IHttpActionResult> UpdateProfileImage()
        {
            var folderName = "uploads";
            var path = HttpContext.Current.Server.MapPath("~/" + folderName);
            var rootUrl = Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.AbsolutePath, String.Empty);
            if (Request.Content.IsMimeMultipartContent())
            {
                var streamProvider = await Request.Content.ReadAsMultipartAsync(new CustomMultipartFormDataStreamProvider(path));
                
                var fileDesc = ContinuationFunction(streamProvider, folderName, rootUrl);

                return Ok(fileDesc);
            }

            return ApiError(HttpStatusCode.NotAcceptable);
        }

        private IEnumerable<FileDesc> ContinuationFunction(CustomMultipartFormDataStreamProvider streamProvider, string folderName, string rootUrl)
        {
            {
                var fileInfo = streamProvider
                    .FileData
                    .Select(i =>
                            {
                                var info = new FileInfo(i.LocalFileName);
                                return new FileDesc
                                       {
                                           Name = info.Name,
                                           Path = rootUrl + "/" + folderName + "/" + info.Name,
                                           Size = info.Length / 1024
                                       };
                            });
                return fileInfo;
            }
        }
    }

    public class FileDesc
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
    }
}