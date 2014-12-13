using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace Api.App
{
    internal interface IHttpContextDependencyProvider
    {
        IContentNegotiator ContentNegotiator { get; set; }
        IEnumerable<MediaTypeFormatter> Formatters { get; set; }
        HttpRequestMessage Request { get; set; }
    }
}