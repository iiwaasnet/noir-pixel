using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace JsonConfigurationProvider
{
    public class ConfigFileReader
    {
        private static readonly JsonSerializerSettings jsonSerializerSettings;

        static ConfigFileReader()
        {
            jsonSerializerSettings = new JsonSerializerSettings
                                     {
                                         ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                         Converters = {new JavaScriptDateTimeConverter()}
                                     };
        }

        public ConfigFileMetadata Parse(FileInfo configFile)
        {
            using (var reader = new StreamReader(configFile.FullName))
            {
                var content = reader.ReadToEnd();

                var sections = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(content, jsonSerializerSettings);
                return new ConfigFileMetadata
                       {
                           Name = configFile.Name,
                           Sections = sections.Select(s => new ConfigSections
                                                           {
                                                               SectionName = s.Keys.First(),
                                                               SectionData = s.Values.First().ToString()
                                                           })
                       };
            }
        }
    }
}