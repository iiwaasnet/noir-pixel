using Api.App.Errors;
using Ext.FluentValidation;
using Ext.FluentValidation.Attributes;
using Ext.FluentValidation.Resources;

namespace Api.App.Auth
{
    [Validator(typeof(LocalAccessTokenModelValidator))]
    public class LocalAccessTokenModel
    {
        public string Provider { get; set; }
        public string ExternalAccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }

    public class LocalAccessTokenModelValidator : AbstractValidator<LocalAccessTokenModel>
    {
        public LocalAccessTokenModelValidator(IResourceAccessorBuilder resourceAccessor)
        {
            RuleFor(m => m.ExternalAccessToken).NotEmpty().WithLocalizedMessage(ApiErrors.Validation.RequiredValue, resourceAccessor);
            RuleFor(m => m.Provider).NotEmpty().WithLocalizedMessage(ApiErrors.Validation.RequiredValue, resourceAccessor);
        }
    }
}