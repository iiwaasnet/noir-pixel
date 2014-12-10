using System.Text.RegularExpressions;
using Api.App.Errors;
using Ext.FluentValidation;
using Ext.FluentValidation.Attributes;
using Ext.FluentValidation.Resources;
using Microsoft.Ajax.Utilities;

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
            var regex = new Regex(@"([a-z0-9_\-]+)", RegexOptions.IgnoreCase);

            RuleFor(m => m.ExternalAccessToken).NotEmpty().WithLocalizedMessage(ApiErrors.Validation.RequiredValue, resourceAccessor);
            RuleFor(m => m.Provider).NotEmpty().WithLocalizedMessage(ApiErrors.Validation.RequiredValue, resourceAccessor);
            RuleFor(m => m.UserName)
                .NotEmpty().WithLocalizedMessage(ApiErrors.Validation.RequiredValue, resourceAccessor)
                .Must(val => ContainValidChars(val, regex)).WithLocalizedMessage(ApiErrors.Validation.InvalidValue, resourceAccessor);

        }

        private bool ContainValidChars(string val, Regex regex)
        {
            var matchResult = regex.Match(val);

            return matchResult.Success && matchResult.Captures.Count == 1;
        }
    }
}