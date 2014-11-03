using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Diagnostics;
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
        private readonly ILogger logger;
        private readonly object @lock = new object();
        private static readonly JsonSerializerSettings jsonSerializerSettings;

        static JsonConfigProvider()
        {
            jsonSerializerSettings = new JsonSerializerSettings
                                     {
                                         ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                         Converters = {new JavaScriptDateTimeConverter()}
                                     };
        }

        public JsonConfigProvider(IConfigTargetProvider targetProvider, ILogger logger)
            : this(targetProvider, "", logger)
        {
        }

        public JsonConfigProvider(IConfigTargetProvider targetProvider, string configBaseDir, ILogger logger)
        {
            this.baseDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configBaseDir);
            this.logger = logger;
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

        public UntypedConfiguration GetUntypedConfiguration(string name)
        {
            return GetUntypedConfiguration(name, target);
        }

        public IEnumerable<UntypedConfiguration> GetAllUntypedConfigurations()
        {
            return GetAllUntypedConfigurations(target);
        }

        public IEnumerable<UntypedConfiguration> GetAllUntypedConfigurations(string target)
        {
            CheckLoadConfiguration();

            return GetConfigNames().Select(sectionName => GetUntypedConfiguration(sectionName, target)).ToArray();
        }

        public UntypedConfiguration GetUntypedConfiguration(string name, string target)
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

            return new UntypedConfiguration
                   {
                       Name = name,
                       Data = stringifiedConfig
                   };
        }

        private IEnumerable<string> GetConfigNames()
        {
            return metadatas.Keys;
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

                    if (section.Target == target || NoExplicitTargetSection(target, metadata))
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
                        if (section.Target == target || NoExplicitTargetSection(target, metadata))
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
            return metadata.Sections.All(s => s.Target != target);
        }

        private void LoadConfiguration()
        {
            var configReader = new ConfigFileReader();
            var locator = new ConfigFileLocator(baseDir);
            var configFiles = locator.FindConfigFiles();

            foreach (var configFile in configFiles)
            {
                try
                {
                    //TODO: FIX problem with exception reading Environment.config.json
                    var metadata = configReader.Parse(configFile);
                    metadatas[metadata.ConfigName.ToLower()] = metadata;
                }
                catch (Exception err)
                {
                    logger.Error(err);
                }
            }
        }

        private void CheckLoadConfiguration()
        {
            if (!configLoaded)
            {
                lock (@lock)
                {
                    if (!configLoaded)
                    {
                        LoadConfiguration();
                        configLoaded = true;
                    }
                }
            }
        }
    }
}