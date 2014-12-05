using System;
using Autofac;
using Ext.FluentValidation;
using Ext.FluentValidation.Resources;
using Ext.FluentValidation.WebApi;

namespace Api.Validation
{
    public class ValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApiResourceBuilder>()
                   .As<IResourceAccessorBuilder>()
                   .SingleInstance();
            builder.Register(c => (Action<FluentValidationModelValidatorProvider>) new WebApiValidation(c.Resolve<IValidatorFactory>()).Configure)
                   .As<Action<FluentValidationModelValidatorProvider>>()
                   .SingleInstance();
            builder.RegisterType<WebApiValidatorFactory>()
                   .As<IValidatorFactory>()
                   .SingleInstance();
            builder.RegisterType<ValidatorActivator>()
                   .As<IValidatorActivator>()
                   .SingleInstance();
        }
    }
}