using System.Collections.Generic;

namespace Api.App.Errors
{
    public class ApiError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public IDictionary<string, object> PlaceholderValues { get; set; }
    }
}