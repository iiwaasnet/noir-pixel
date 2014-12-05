namespace Api.App.Errors
{
    public class ApiErrors
    {
        public class Auth
        {
            public static string ExternalLoginDataNotFound = "EAPI_Auth_ExternalLoginDataNotFound";
            public static string InvalidProviderOrAccessToken = "EAPI_Auth_InvalidProviderOrAccessToken";
        }

        public class Validation
        {
            public static string InvalidModelState = "EAPI_InvalidModelState";
            public static string RequiredValue = "EAPI_ValueRequired";
            public static string InvalidValue = "EAPI_InvalidValue";
            public static string NotSame = "EAPI_NotSame";
            public static string InvalidEmail = "EAPI_InvalidMail";
        }

        public static string UnknownError = "EAPI_UnknownError";
        public static string InternalError = "EAPI_InternalError";
    }
}