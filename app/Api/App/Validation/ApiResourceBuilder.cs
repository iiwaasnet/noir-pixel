using System;
using Ext.FluentValidation.Resources;
using Resources.Api;

namespace Api.App.Validation
{
    public class ApiResourceBuilder : IResourceAccessorBuilder
    {
        private readonly IApiStringsProvider stringsProvider;

        public ApiResourceBuilder(IApiStringsProvider stringsProvider)
        {
            this.stringsProvider = stringsProvider;
        }

        public Func<string> GetResourceAccessor(Type resourceType, string resourceName)
        {
            return () => stringsProvider.GetString(resourceName);
        }
    }
}