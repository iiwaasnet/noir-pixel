namespace Api.App.Errors
{
    public class FieldValidationError
    {
        public string Code { get; set; }
        public string Field { get; set; }
        public string Message { get; set; }
    }
}