using System;

namespace Api.Validation
{
    public interface IValidatorActivator
    {
        object Activate(Type objType);
    }
}