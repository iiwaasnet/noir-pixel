using System.Collections.Generic;

namespace Resources.Client
{
    public interface IClientStringsProvider
    {
        IEnumerable<KeyValuePair<string, string>> GetLocalizedStrings(string lang);
        string GetCurrentVersion();
    }
}