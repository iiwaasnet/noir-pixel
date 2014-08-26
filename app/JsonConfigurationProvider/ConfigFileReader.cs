using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace JsonConfigurationProvider
{
    public class ConfigFileReader
    {
        public ConfigFileMetadata Parse(FileInfo configFile)
        {
            using (var reader = new StreamReader(configFile.FullName))
            {
                var content = reader.ReadToEnd();

                var sections = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(content);
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