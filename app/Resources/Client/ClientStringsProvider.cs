using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Resources.Client
{
    public class ClientStringsProvider : IClientStringsProvider
    {
        public IEnumerable<KeyValuePair<string, string>> GetLocalizedStrings(string lang)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(lang);
            var resourceSet = Strings.ResourceManager.GetResourceSet(cultureInfo, true, true);

            return GetStringsFromResourceSet(resourceSet).ToArray();
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