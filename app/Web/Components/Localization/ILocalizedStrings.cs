using System.Collections.Generic;

namespace Web.Components.Localization
{
    public interface ILocalizedStrings
    {
        LocalizedStringCollection GetLocalizedCollection(string locale);
        LocalizedStringCollection GetDefaultCollection();
        IEnumerable<string> GetSupportedLocales();
    }
}