using System.Collections.Generic;

namespace Web.Components.Localization
{
    public interface ILocalizedStrings
    {
        string GetCurrentVersion();
        LocalizedStringCollection GetLocalizedCollection(string locale);
        LocalizedStringCollection GetDefaultCollection();
        IEnumerable<string> GetSupportedLocales();
    }
}