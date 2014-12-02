using System.Collections.Generic;

namespace Api.App.Errors
{
    public class ValidationError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public IEnumerable<FieldValidationError> Errors { get; set; }
    }
}