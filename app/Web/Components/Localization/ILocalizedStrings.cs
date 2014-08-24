using System.Collections.Generic;

namespace Web.Components.Localization
{
    public interface ILocalizedStrings
    {
        LocalizedStringCollection GetLocalizedCollection(string locale);
        IEnumerable<string> GetSupportedLocales();
    }
}