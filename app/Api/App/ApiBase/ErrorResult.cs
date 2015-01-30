using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.App.ApiBase
{
    public class ErrorResult : IHttpActionResult
    {
        private readonly HttpRequestMessage request;
        private readonly HttpStatusCode statusCode;

        public ErrorResult(HttpStatusCode statusCode, ApiController controller)
        {
            this.statusCode = statusCode;
            request = controller.Request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return new HttpResponseMessage {StatusCode = statusCode, RequestMessage = request};
        }
    }
}