using System;

namespace Api.App.Validation
{
    public interface IValidatorActivator
    {
        object Activate(Type objType);
    }
}