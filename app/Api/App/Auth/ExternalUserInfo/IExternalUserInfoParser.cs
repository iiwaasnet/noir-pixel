namespace Api.App.Auth.ExternalUserInfo
{
    public interface IExternalUserInfoParser
    {
        ExternalUserInfo Parse(string provdier, dynamic userObj);
    }
}