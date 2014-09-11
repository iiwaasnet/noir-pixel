using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonConfigurationProvider
{
    public class JsonConfigTargetProvider : IConfigTargetProvider
    {
        public class TargetEnvironment
        {
            public string Environment { get; set; }
        }

        private readonly string fileName;
        public JsonConfigTargetProvider(string fileName)
        {
            this.fileName = fileName;
        }

        public string GetCurrentTarget()
        {

            using (var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)))
            {
                var content = reader.ReadToEnd();
                var env = JsonConvert.DeserializeObject<TargetEnvironment>(content, new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});

                return env.Environment;
            }
        }
    }
}