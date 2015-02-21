using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Components.Common
{
    public static class RequestExtensions
    {
        private static readonly IEnumerable<string> NullLayoutPaths;

        static RequestExtensions()
        {
            NullLayoutPaths = new[]
                              {
                                  "/external-signin",
                                  "/external-register"
                              };
        }

        public static bool RenderMainLayout(this HttpRequestBase request)
        {
            return NullLayoutPaths.All(path => request.Path.ToLower() != path);
        }
    }
}