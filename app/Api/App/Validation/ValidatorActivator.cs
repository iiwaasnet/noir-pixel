using System;
using System.Linq;
using Autofac;

namespace Api.App.Validation
{
    public class ValidatorActivator : IValidatorActivator
    {
        private readonly IContainer dependenciesContainer;

        public ValidatorActivator()
        {
            dependenciesContainer = DependencyInjection.GetContainer();
        }

        public object Activate(Type objType)
        {
            var ctors = objType.GetConstructors();
            var ctor = ctors.First();
            var @params = ctor.GetParameters();

            return Activator.CreateInstance(objType, @params.Select(p => dependenciesContainer.Resolve(p.ParameterType)).ToArray());
        }
    }
}