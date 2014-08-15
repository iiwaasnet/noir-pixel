using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;

namespace Web.Components
{
    public static class OwinHeadersExtensions
    {
        public static IDictionary<string, string> ToHttpHeaders(this IHeaderDictionary headers)
        {
            return headers.Keys.ToDictionary(key => key, headers.Get);
        }
    }
}