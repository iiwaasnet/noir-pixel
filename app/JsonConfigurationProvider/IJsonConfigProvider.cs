namespace JsonConfigurationProvider
{
    public interface IJsonConfigProvider
    {
        T GetConfiguration<T>()
            where T : class, new();

        T GetConfiguration<T>(string target)
            where T : class, new();
    }
}