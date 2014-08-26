using System;
using System.Collections.Concurrent;
using System.Linq;
using Newtonsoft.Json;

namespace JsonConfigurationProvider
{
    public class JsonConfigProvider : IJsonConfigProvider
    {
        private readonly string baseDir;
        private readonly string target;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<Type, object>> configurations;
        private readonly ConcurrentDictionary<string, ConfigFileMetadata> metadatas;

        private volatile bool configLoaded;

        public JsonConfigProvider(IConfigTargetProvider targetProvider)
            : this(targetProvider, AppDomain.CurrentDomain.BaseDirectory)
        {
        }

        public JsonConfigProvider(IConfigTargetProvider targetProvider, string baseDir)
        {
            this.baseDir = baseDir;
            target = targetProvider.GetCurrentTarget();
            configurations = new ConcurrentDictionary<string, ConcurrentDictionary<Type, object>>();
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

            if (!configLoaded)
            {
                LoadConfiguration();
                configLoaded = true;
            }

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
                        JsonConvert.PopulateObject(section.SectionData, config);
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
                if (metadatas.ContainsKey(metadata.Name.ToLower()))
                {
                    throw new Exception(string.Format("Configuration {0} is already loaded!", metadata.Name));
                }
                metadatas[metadata.Name.ToLower()] = metadata;
            }
        }
    }
}