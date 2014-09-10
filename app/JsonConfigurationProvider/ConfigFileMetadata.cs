using System.Collections.Generic;

namespace JsonConfigurationProvider
{
    public class ConfigFileMetadata
    {
        public string ConfigName { get; set; }
        public IEnumerable<ConfigSections> Sections { get; set; }
    }
}