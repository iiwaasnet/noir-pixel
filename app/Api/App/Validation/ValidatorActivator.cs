﻿using System;
using Ext.FluentValidation.Resources;

namespace Api.App.Validation
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