using System.Collections.Generic;

namespace Resources
{
    public interface IStringsProvider
    {
        IEnumerable<KeyValuePair<string, string>> GetLocalizedStrings(string lang);
    }
}