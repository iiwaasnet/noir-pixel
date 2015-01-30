using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.App.ApiBase
{
    public class FileResult : IHttpActionResult
    {
        private readonly string fileName;
        private readonly string contentType;
        private const string ImageMime = "image/jpeg";

        public FileResult(string fileName, string contentType = null)
        {
            // TODO: This should be changed, when more than one file format should be supported
            this.contentType = (string.IsNullOrWhiteSpace(contentType))
                                   ? ImageMime
                                   : contentType;
            this.fileName = fileName;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            if (!File.Exists(fileName))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                response.Content = new StreamContent(new FileStream(fileName, FileMode.Open, FileAccess.Read));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                return response;
            }
            catch
            {
                response.Dispose();
                throw;
            }
        }
    }
}