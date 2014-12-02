namespace Api.App.Errors
{
    public class ApiErrors
    {
        public class Auth
        {
            public const string ExternalLoginDataNotFound = "EAPI_Auth_ExternalLoginDataNotFound";
        }

        public const string UnknownError = "EAPI_UnknownError";
        public const string InternalError = "EAPI_InternalError";
        public const string InvalidModelState = "EAPI_InvalidModelState";
    }
}