namespace Resources.Api
{
    public class ApiStringsProvider : IApiStringsProvider
    {
        public string GetString(string id)
        {
            var str = Strings.ResourceManager.GetString(id);

            return (!string.IsNullOrWhiteSpace(str)) ? str : id;
        }
    }
}