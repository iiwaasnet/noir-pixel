using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api.App
{
    public class ApiErrorResult<T> : IHttpActionResult
    {
        private readonly T content;
        private readonly IContentNegotiator contentNegotiator;
        private readonly IEnumerable<MediaTypeFormatter> formatters;
        private readonly HttpRequestMessage request;
        private readonly HttpStatusCode statusCode;

        internal ApiErrorResult(HttpStatusCode statusCode, T content, IHttpContextDependencyProvider dependencyProvider)
        {
            contentNegotiator = dependencyProvider.ContentNegotiator;
            formatters = dependencyProvider.Formatters;
            request = dependencyProvider.Request;
            this.statusCode = statusCode;
            this.content = content;
        }

        public ApiErrorResult(HttpStatusCode statusCode, T content, ApiController controller)
        {
            this.content = content;
            this.statusCode = statusCode;
            contentNegotiator = controller.Configuration.Services.GetContentNegotiator();
            formatters = controller.Configuration.Formatters;
            request = controller.Request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }


        private HttpResponseMessage Execute()
        {
            var result = contentNegotiator.Negotiate(typeof (T), request, formatters);

            var response = new HttpResponseMessage();
            try
            {
                if (result == null)
                {
                    response.StatusCode = HttpStatusCode.NotAcceptable;
                }
                else
                {
                    response.StatusCode = statusCode;
                    response.Content = new ObjectContent<T>(content, result.Formatter, result.MediaType);
                }

                response.RequestMessage = request;
            }
            catch
            {
                response.Dispose();
                throw;
            }

            return response;
        }
    }
}