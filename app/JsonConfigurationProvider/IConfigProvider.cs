namespace JsonConfigurationProvider
{
    public interface IConfigProvider
    {
        T GetConfiguration<T>()
            where T : class, new();

        T GetConfiguration<T>(string target)
            where T : class, new();

        string GetUntypedConfiguration(string name);
        string GetUntypedConfiguration(string name, string target);
    }
}