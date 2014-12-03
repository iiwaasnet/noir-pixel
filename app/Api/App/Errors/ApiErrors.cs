namespace Api.App.Errors
{
    public class ApiErrors
    {
        public class Auth
        {
            public const string ExternalLoginDataNotFound = "EAPI_Auth_ExternalLoginDataNotFound";
            public const string InvalidProviderOrAccessToken = "EAPI_Auth_InvalidProviderOrAccessToken";
        }

        public class Validation
        {
            public const string InvalidModelState = "EAPI_InvalidModelState";
            public const string RequiredValue = "EAPI_ValueRequired";
            public const string InvalidValue = "EAPI_InvalidValue";
            public const string NotSame = "EAPI_NotSame";
            public const string InvalidEmail = "EAPI_InvalidMail";
        }

        public const string UnknownError = "EAPI_UnknownError";
        public const string InternalError = "EAPI_InternalError";
    }
}