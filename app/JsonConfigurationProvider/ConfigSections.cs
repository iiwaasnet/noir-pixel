namespace JsonConfigurationProvider
{
    public class ConfigSections
    {
        private string sectionName;

        public string SectionName
        {
            get { return sectionName; }
            set { sectionName = value.ToLower(); }
        }

        public string SectionData { get; set; }
    }
}