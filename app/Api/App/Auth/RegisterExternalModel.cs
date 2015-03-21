using System.Text.RegularExpressions;
using Api.App.Auth.Config;
using Api.App.Errors;
using Ext.FluentValidation;
using Ext.FluentValidation.Attributes;
using Ext.FluentValidation.Resources;
using TypedConfigProvider;

namespace Api.App.Auth
{
    [Validator(typeof(RegisterExternalModelValidator))]
    public class RegisterExternalModel
    {
        public string Provider { get; set; }
        public string ExternalAccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalModelValidator : AbstractValidator<RegisterExternalModel>
    {
        public RegisterExternalModelValidator(IResourceAccessorBuilder resourceAccessor, IConfigProvider configProvider)
        {
            var authConfig = configProvider.GetConfiguration<AuthConfiguration>();

            var regex = new Regex(authConfig.UserValidation.NameValidationRegEx, RegexOptions.IgnoreCase);

            RuleFor(m => m.ExternalAccessToken).NotEmpty().WithLocalizedMessage(ApiErrors.Validation.RequiredValue, resourceAccessor);
            RuleFor(m => m.Provider).NotEmpty().WithLocalizedMessage(ApiErrors.Validation.RequiredValue, resourceAccessor);
            RuleFor(m => m.UserName)
                .NotEmpty().WithLocalizedMessage(ApiErrors.Validation.RequiredValue, resourceAccessor)
                .Must(val => ContainValidChars(val, regex)).WithLocalizedMessage(ApiErrors.Validation.InvalidValue, resourceAccessor)
                .Length(authConfig.UserValidation.MinUserNameLength, authConfig.UserValidation.MaxUSerNameLength).WithLocalizedMessage(ApiErrors.Validation.ValueLength, resourceAccessor);

        }

        private static bool ContainValidChars(string val, Regex regex)
        {
            return regex.Match(val).Success;
        }
    }
}