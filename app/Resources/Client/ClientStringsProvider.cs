using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security.Cryptography;
using Diagnostics;

namespace Resources.Client
{
    public class ClientStringsProvider : IClientStringsProvider
    {
        private readonly string resourcesVersion;
        private readonly ILogger logger;

        public ClientStringsProvider(ILogger logger)
        {
            this.logger = logger;
            resourcesVersion = TryGetVersion();
        }

        public IEnumerable<KeyValuePair<string, string>> GetLocalizedStrings(string lang)
        {
            var cultureInfo = CultureInfo.CreateSpecificCulture(lang);
            var resourceSet = Strings.ResourceManager.GetResourceSet(cultureInfo, true, true);

            return GetStringsFromResourceSet(resourceSet).ToArray();
        }

        public string GetCurrentVersion()
        {
            return resourcesVersion;
        }

        private string TryGetVersion()
        {
            try
            {
                var sha = new SHA256Managed();
                var hash = sha.ComputeHash(File.OpenRead(GetType().Assembly.Location));

                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
            catch (Exception err)
            {
                logger.Error(err);

                return Guid.NewGuid().ToString();
            }
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