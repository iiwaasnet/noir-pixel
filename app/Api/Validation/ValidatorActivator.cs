using System;
using FluentValidation.Resources;

namespace Api.Validation
{
    public class ValidatorActivator : IValidatorActivator
    {
        private readonly IResourceAccessorBuilder resourceAccessorBuilder;

        public ValidatorActivator(IResourceAccessorBuilder resourceAccessorBuilder)
        {
            this.resourceAccessorBuilder = resourceAccessorBuilder;
        }

        public object Activate(Type objType)
        {
            return Activator.CreateInstance(objType, resourceAccessorBuilder);
        }
    }
}