using System;
using Ext.FluentValidation;
using Ext.FluentValidation.Attributes;

namespace Api.App.Validation
{
    public class WebApiValidatorFactory : IValidatorFactory
    {
        private readonly ValidatorCache cache;
        private readonly IValidatorActivator validatorActivator;

        public WebApiValidatorFactory(IValidatorActivator validatorActivator)
        {
            cache = new ValidatorCache();
            this.validatorActivator = validatorActivator;
        }

        public IValidator<T> GetValidator<T>()
        {
            return (IValidator<T>) GetValidator(typeof (T));
        }

        public virtual IValidator GetValidator(Type type)
        {
            if (type == null)
            {
                return null;
            }

            var attribute = GetValidatorAttribute(type);

            if (attribute == null || attribute.ValidatorType == null)
            {
                return null;
            }

            return cache.GetOrCreateInstance(attribute.ValidatorType, validatorActivator.Activate) as IValidator;
        }

        private static ValidatorAttribute GetValidatorAttribute(Type type)
        {
            var attribute = (ValidatorAttribute) Attribute.GetCustomAttribute(type, typeof (ValidatorAttribute));

            return attribute;
        }
    }
}