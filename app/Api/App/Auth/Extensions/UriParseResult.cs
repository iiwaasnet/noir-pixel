using Api.App.Errors;

namespace Api.App.Auth.Extensions
{
    public class UriParseResult
    {
        public string Uri { get; set; }
        public bool Parsed { get; set; }
        public ValidationError Error { get; set; }
    }
}