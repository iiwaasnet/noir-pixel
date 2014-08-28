using System.Collections.Generic;
using System.Linq;
using Resources;

namespace Web.Components.Localization
{
    public class LocalizedStrings : ILocalizedStrings
    {
        private const string DefaultLocale = "en";
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
                       Version = "1",
                       Strings = stringsProvider
                           .GetLocalizedStrings(locale)
                           .Select(str => new LocalizedString {Key = str.Key, Value = str.Value})
                           .ToArray()
                   };
        }

        public LocalizedStringCollection GetDefaultCollection()
        {
            return GetLocalizedCollection(DefaultLocale);
        }

        public IEnumerable<string> GetSupportedLocales()
        {
            yield return DefaultLocale;
            yield return "ru";
        }
    }
}