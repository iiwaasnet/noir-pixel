namespace JsonConfigurationProvider
{
    public class ConfigSections
    {
        private string target;

        public string Target
        {
            get { return target; }
            set { target = value.ToLower(); }
        }

        public string SectionData { get; set; }
    }
}