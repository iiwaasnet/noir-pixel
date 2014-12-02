using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Resources
{
    public class StringsProvider : IStringsProvider
    {
        public IEnumerable<KeyValuePair<string, string>> GetLocalizedStrings(string lang)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(lang);
            var resourceSet = Rs.ResourceManager.GetResourceSet(cultureInfo, true, true);

            return GetStringsFromResourceSet(resourceSet).ToArray();
        }

        public string GetString(string id)
        {
            return Rs.ResourceManager.GetString(id);
        }

        private IEnumerable<KeyValuePair<string, string>> GetStringsFromResourceSet(ResourceSet resourceSet)
        {
            var enumerator = resourceSet.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = (DictionaryEntry) enumerator.Current;
                yield return new KeyValuePair<string, string>(entry.Key.ToString(), entry.Value.ToString());
            }
        }
    }
}