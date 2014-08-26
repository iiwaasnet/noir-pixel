using System;
using System.IO;

namespace JsonConfigurationProvider
{
    public class ConfigTargetProvider : IConfigTargetProvider
    {
        private readonly string fileName;

        public ConfigTargetProvider(string fileName)
        {
            this.fileName = fileName;
        }

        public string GetCurrentTarget()
        {
            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, fileName)))
            {
                return reader.ReadToEnd().Trim();
            }
        }
    }
}