namespace Api.App.Auth.Config
{
    public class UserValidation
    {
        public string NameValidationRegEx { get; set; }
        public int MinUserNameLength { get; set; }
        public int MaxUSerNameLength { get; set; }
    }
}