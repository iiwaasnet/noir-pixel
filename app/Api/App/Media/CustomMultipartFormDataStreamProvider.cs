using System.Net.Http;
using System.Net.Http.Headers;

namespace Api.App.Media
{
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        public CustomMultipartFormDataStreamProvider(string path)
            : base(path)
        {
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            var name = !string.IsNullOrWhiteSpace(headers.ContentDisposition.FileName) ? headers.ContentDisposition.FileName : "NoName";
            return RemoveChromeFileNameQuatation(name);
        }

        private static string RemoveChromeFileNameQuatation(string name)
        {
            return name.Replace("\"", string.Empty);
        }
    }
}