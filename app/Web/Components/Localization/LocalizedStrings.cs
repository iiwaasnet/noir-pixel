using System.Collections.Generic;
using System.Linq;
using Resources;

namespace Web.Components.Localization
{
    public class LocalizedStrings : ILocalizedStrings
    {
        private readonly IStringsProvider stringsProvider;
        public LocalizedStrings(IStringsProvider stringsProvider)
        {
            this.stringsProvider = stringsProvider;
        }


        public LocalizedStringCollection GetLocalizedCollection(string locale)
        {
            return new LocalizedStringCollection
                   {
                       Locale = locale,
                       Strings = stringsProvider
                           .GetLocalizedStrings(locale)
                           .Select(str => new LocalizedString {Key = str.Key, Value = str.Value})
                           .ToArray()
                   };
        }

        public IEnumerable<string> GetSupportedLocales()
        {
            yield return "en";
            yield return "ru";
        }
    }
}