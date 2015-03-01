namespace Api.App.Errors
{
    public class ApiErrors
    {
        public const string UnknownError = "EAPI_UnknownError";
        public const string InternalError = "EAPI_InternalError";

        public class Auth
        {
            public const string ExternalLoginDataNotFound = "EAPI_Auth_ExternalLoginDataNotFound";
            public const string InvalidProviderOrAccessToken = "EAPI_Auth_InvalidProviderOrAccessToken";
            public const string InvalidExternalAccessToken = "EAPI_Auth_InvalidExternalAccessToken";
            public const string UserAlreadyRegistered = "EAPI_Auth_UserAlreadyRegistered";
            public const string UserNotRegistered = "EAPI_Auth_UserNotRegistered";
            public const string AuthError = "EAPI_Auth_Error";
        }

        public class Validation
        {
            public const string InvalidModelState = "EAPI_InvalidModelState";
            public const string RequiredValue = "EAPI_ValueRequired";
            public const string InvalidValue = "EAPI_InvalidValue";
            public const string ValueLength = "EAPI_ValueLength";
            public const string NotSame = "EAPI_NotSame";
            public const string InvalidEmail = "EAPI_InvalidMail";
        }

        public class Media
        {
            public const string UsupportedMediaFormat = "EAPI_Media_UsupportedFormat";
            public const string FileTooBig = "EAPI_Media_FileTooBig";
        }
    }
}