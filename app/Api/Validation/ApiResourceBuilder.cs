﻿using System;
using Ext.FluentValidation.Resources;

namespace Api.Validation
{
    public class ApiResourceBuilder : IResourceAccessorBuilder
    {
        public Func<string> GetResourceAccessor(Type resourceType, string resourceName)
        {
            return () => resourceName;
        }
    }
}