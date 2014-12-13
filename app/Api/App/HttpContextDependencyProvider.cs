using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace Api.App
{
    internal class HttpContextDependencyProvider : IHttpContextDependencyProvider
    {
        public IContentNegotiator ContentNegotiator { get; set; }
        public IEnumerable<MediaTypeFormatter> Formatters { get; set; }
        public HttpRequestMessage Request { get; set; }
    }
}