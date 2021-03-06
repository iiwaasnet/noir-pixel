﻿using System.Collections.Generic;

namespace JsonConfigurationProvider
{
    public interface IConfigProvider
    {
        T GetConfiguration<T>()
            where T : class, new();

        T GetConfiguration<T>(string target)
            where T : class, new();

        UntypedConfiguration GetUntypedConfiguration(string name);
        UntypedConfiguration GetUntypedConfiguration(string name, string target);
        IEnumerable<UntypedConfiguration> GetAllUntypedConfigurations();
        IEnumerable<UntypedConfiguration> GetAllUntypedConfigurations(string target);
    }
}