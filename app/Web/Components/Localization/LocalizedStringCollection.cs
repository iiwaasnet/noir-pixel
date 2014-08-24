using System.Collections.Generic;

namespace Web.Components.Localization
{
    public class LocalizedStringCollection
    {
        public string Locale { get; set; }
        public IEnumerable<LocalizedString> Strings { get; set; }
    }
}