using Api.App.Errors;
using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Resources;

namespace Api.App.Auth
{
    [Validator(typeof(RegisterExternalModelValidator))]
    public class RegisterExternalModel
    {
        public string Provider { get; set; }
        public string ExternalAccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }

    public class RegisterExternalModelValidator : AbstractValidator<RegisterExternalModel>
    {
        public RegisterExternalModelValidator(IResourceAccessorBuilder resourceAccessor)
        {
            RuleFor(m => m.ExternalAccessToken).NotEmpty().WithLocalizedMessage(() => ApiErrors.Validation.RequiredValue, resourceAccessor);
            RuleFor(m => m.Provider).NotEmpty().WithLocalizedMessage(() => ApiErrors.Validation.RequiredValue, resourceAccessor);
        }
    }
}