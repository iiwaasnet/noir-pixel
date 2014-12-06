using System;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using System.Web.Http.Validation;

namespace Api.App.Validation
{
    public class PrefixlessModelValidator : IBodyModelValidator
    {
        private readonly IBodyModelValidator innerValidator;

        public PrefixlessModelValidator(IBodyModelValidator innerValidator)
        {
            if (innerValidator == null)
            {
                throw new ArgumentNullException("innerValidator");
            }

            this.innerValidator = innerValidator;
        }

        public bool Validate(object model, Type type, ModelMetadataProvider metadataProvider, HttpActionContext actionContext, string keyPrefix)
        {
            // Remove the keyPrefix but otherwise let innerValidator do what it normally does.
            return innerValidator.Validate(model, type, metadataProvider, actionContext, string.Empty);
        }
    }
}