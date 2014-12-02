namespace Resources.Api
{
    public class ApiStringsProvider : IApiStringsProvider
    {
        public string GetString(string id)
        {
            return Strings.ResourceManager.GetString(id);
        }
    }
}