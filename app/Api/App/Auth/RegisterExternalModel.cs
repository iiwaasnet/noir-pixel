using System.Text.RegularExpressions;
using Api.App.Errors;
using Ext.FluentValidation;
using Ext.FluentValidation.Attributes;
using Ext.FluentValidation.Resources;

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
        public RegisterExternalModelValidator(IResourceAccessorBuilder resourceAccessor)
        {
            var regex = new Regex(@"(^\b[a-z0-9_\-]+\b$)", RegexOptions.IgnoreCase);

            RuleFor(m => m.ExternalAccessToken).NotEmpty().WithLocalizedMessage(ApiErrors.Validation.RequiredValue, resourceAccessor);
            RuleFor(m => m.Provider).NotEmpty().WithLocalizedMessage(ApiErrors.Validation.RequiredValue, resourceAccessor);
            RuleFor(m => m.UserName)
                .NotEmpty().WithLocalizedMessage(ApiErrors.Validation.RequiredValue, resourceAccessor)
                .Must(val => ContainValidChars(val, regex)).WithLocalizedMessage(ApiErrors.Validation.InvalidValue, resourceAccessor);

        }

        private static bool ContainValidChars(string val, Regex regex)
        {
            return regex.Match(val).Success;
        }
    }
}