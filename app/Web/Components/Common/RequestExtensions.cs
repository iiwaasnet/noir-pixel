using System.Web;

namespace Web.Components.Common
{
    public static class RequestExtensions
    {
        private const string ExternalSignin = "/external-signin";

        public static bool ShowHeader(this HttpRequestBase request)
        {
            return request.Path.ToLower() != ExternalSignin;
        }
        
        public static bool ShowFooter(this HttpRequestBase request)
        {
            return request.Path.ToLower() != ExternalSignin;
        }
    }
}