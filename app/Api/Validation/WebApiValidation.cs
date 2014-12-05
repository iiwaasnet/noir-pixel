using Ext.FluentValidation;
using Ext.FluentValidation.WebApi;

namespace Api.Validation
{
    public class WebApiValidation
    {
        private readonly IValidatorFactory validationFactory;

        public WebApiValidation(IValidatorFactory validationFactory)
        {
            this.validationFactory = validationFactory;
        }

        public void Configure(FluentValidationModelValidatorProvider provider)
        {
            provider.ValidatorFactory = validationFactory;
        }
    }
}