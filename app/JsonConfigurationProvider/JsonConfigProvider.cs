using System;
using System.Collections.Concurrent;
using System.Linq;
using Newtonsoft.Json;

namespace JsonConfigurationProvider
{
    public class JsonConfigProvider
    {
        private readonly string baseDir;
        private readonly string target;
        private readonly ConcurrentDictionary<string, ConcurrentDictionary<Type, object>> configurations;
        private readonly ConcurrentDictionary<string, ConfigFileMetadata> metadatas;

        private volatile bool configLoaded;

        public JsonConfigProvider(string target)
            : this(target, AppDomain.CurrentDomain.BaseDirectory)
        {
        }

        public JsonConfigProvider(string target, string baseDir)
        {
            this.baseDir = baseDir;
            this.target = target;
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
            if (!configLoaded)
            {
                LoadConfiguration();
            }

            ConcurrentDictionary<Type, object> configObjects;
            if (!configurations.TryGetValue(target, out configObjects))
            {
                var config = TryGetConfiguration<T>(target);
            }
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
                        if (section.SectionName == target || metadata.Sections.All(s => s.SectionName != target))
                        {
                            return config;
                        }
                    }
                }
                catch
                {
                }
            }
            //foreach (var metadata in metadatas.Values)
            //{
            //    try
            //    {
            //        var config = new T();
            //        var section = metadata.Sections.First();
            //        JsonConvert.PopulateObject(section.SectionData, config);
            //        return config;
            //    }
            //    catch
            //    {
            //    }
            //}

            return default(T);
        }

        private void LoadConfiguration()
        {
            var configReader = new ConfigFileReader();
            var locator = new ConfigFileLocator(baseDir);
            var configFiles = locator.FindConfigFiles();

            foreach (var metadata in configFiles.Select(configReader.Parse))
            {
                if (metadatas.ContainsKey(metadata.Name))
                {
                    throw new Exception(string.Format("Configuration {0} is already loaded!", metadata.Name));
                }
                metadatas[metadata.Name] = metadata;
            }
        }
    }
}