using System;
using System.Collections.Concurrent;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace JsonConfigurationProvider
{
    public class JsonConfigProvider : IConfigProvider
    {
        private readonly string baseDir;
        private readonly string target;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<Type, object>> configurations;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, string>> untypedConfigurations;
        private readonly ConcurrentDictionary<string, ConfigFileMetadata> metadatas;
        private volatile bool configLoaded;
        private static readonly JsonSerializerSettings jsonSerializerSettings;

        static JsonConfigProvider()
        {
            jsonSerializerSettings = new JsonSerializerSettings
                                     {
                                         ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                         Converters = {new JavaScriptDateTimeConverter()}
                                     };
        }

        public JsonConfigProvider(IConfigTargetProvider targetProvider)
            : this(targetProvider, AppDomain.CurrentDomain.BaseDirectory)
        {
        }

        public JsonConfigProvider(IConfigTargetProvider targetProvider, string baseDir)
        {
            this.baseDir = baseDir;
            target = targetProvider.GetCurrentTarget();
            configurations = new ConcurrentDictionary<string, ConcurrentDictionary<Type, object>>();
            untypedConfigurations = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
            metadatas = new ConcurrentDictionary<string, ConfigFileMetadata>();
            configLoaded = false;
        }

        public T GetConfiguration<T>()
            where T : class, new()
        {
            return GetConfiguration<T>(target);
        }

        public T GetConfiguration<T>(string target)
            where T : class, new()
        {
            target = target.ToLower();

            CheckLoadConfiguration();

            ConcurrentDictionary<Type, object> configObjects;
            T config;
            if (!configurations.TryGetValue(target, out configObjects) || (config = TryGetConfiguration<T>(configObjects)) == null)
            {
                config = TryGetConfiguration<T>(target);
                if (config == null)
                {
                    throw new Exception(string.Format("Unable to get configuration of type {0} for target {1}!", typeof (T).Name, target));
                }

                if (configObjects == null)
                {
                    configObjects = new ConcurrentDictionary<Type, object>();
                    configurations[target] = configObjects;
                }
                configObjects[typeof (T)] = config;
            }

            return config;
        }

        public string GetUntypedConfiguration(string name)
        {
            return GetUntypedConfiguration(name, target);
        }

        public string GetUntypedConfiguration(string name, string target)
        {
            target = target.ToLower();
            name = name.ToLower();

            CheckLoadConfiguration();

            string stringifiedConfig;
            ConcurrentDictionary<string, string> configObjects;
            if (!untypedConfigurations.TryGetValue(target, out configObjects) || string.IsNullOrEmpty(stringifiedConfig = TryGetUntypedConfiguration(configObjects, name)))
            {
                stringifiedConfig = TryGetUntypedConfiguration(target, name);
                if (string.IsNullOrEmpty(stringifiedConfig))
                {
                    throw new Exception(string.Format("Unable to get configuration named {0} for target {1}!", name, target));
                }

                if (configObjects == null)
                {
                    configObjects = new ConcurrentDictionary<string, string>();
                    untypedConfigurations[target] = configObjects;
                }
                configObjects[name] = stringifiedConfig;
            }

            return stringifiedConfig;
        }

        private string TryGetUntypedConfiguration(string target, string name)
        {
            var config = new JObject();
            ConfigFileMetadata metadata;

            if (metadatas.TryGetValue(name, out metadata))
            {
                foreach (var section in metadata.Sections)
                {
                    var tmp = JObject.Parse(section.SectionData);
                    config.Merge(tmp, new JsonMergeSettings {MergeArrayHandling = MergeArrayHandling.Replace});

                    if (section.SectionName == target || NoExplicitTargetSection(target, metadata))
                    {
                        return config.ToString();
                    }
                }
            }

            return null;
        }

        private static string TryGetUntypedConfiguration(ConcurrentDictionary<string, string> configObjects, string name)
        {
            string config;
            configObjects.TryGetValue(name, out config);

            return config;
        }

        private static T TryGetConfiguration<T>(ConcurrentDictionary<Type, object> configObjects)
        {
            object tmpConfig;
            configObjects.TryGetValue(typeof (T), out tmpConfig);

            return (T) tmpConfig;
        }

        private T TryGetConfiguration<T>(string target)
            where T : class, new()
        {
            foreach (var metadata in metadatas.Values)
            {
                try
                {
                    var config = new T();
                    foreach (var section in metadata.Sections)
                    {
                        JsonConvert.PopulateObject(section.SectionData, config, jsonSerializerSettings);
                        if (section.SectionName == target || NoExplicitTargetSection(target, metadata))
                        {
                            return config;
                        }
                    }
                }
                catch
                {
                }
            }

            return default(T);
        }

        private static bool NoExplicitTargetSection(string target, ConfigFileMetadata metadata)
        {
            return metadata.Sections.All(s => s.SectionName != target);
        }

        private void LoadConfiguration()
        {
            var configReader = new ConfigFileReader();
            var locator = new ConfigFileLocator(baseDir);
            var configFiles = locator.FindConfigFiles();

            foreach (var metadata in configFiles.Select(configReader.Parse))
            {
                metadatas[metadata.Name.ToLower()] = metadata;
            }
        }

        private void CheckLoadConfiguration()
        {
            if (!configLoaded)
            {
                LoadConfiguration();
                configLoaded = true;
            }
        }
    }
}