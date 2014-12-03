using System.Collections.Generic;
using System.Linq;
using System.Web.Http.ModelBinding;
using Resources.Api;

namespace Api.App.Errors.Extensions
{
    public static class ModelStateExtensions
    {
        public static ValidationError ToValidationError(this IEnumerable<KeyValuePair<string, ModelState>> modelState, IApiStringsProvider stringsProvider)
        {
            return new ValidationError
                   {
                       Code = ApiErrors.Validation.InvalidModelState,
                       Message = stringsProvider.GetString(ApiErrors.Validation.InvalidModelState),
                       Errors = CreateValidationErrors(modelState, stringsProvider)
                   };
        }

        private static IEnumerable<FieldValidationError> CreateValidationErrors(IEnumerable<KeyValuePair<string, ModelState>> modelState, IApiStringsProvider stringsProvider)
        {
            return modelState.SelectMany(prop => prop.Value
                                                     .Errors
                                                     .Select(e => new FieldValidationError
                                                                  {
                                                                      Field = prop.Key,
                                                                      Code = e.ErrorMessage,
                                                                      Message = stringsProvider.GetString(e.ErrorMessage)
                                                                  }));
        }
    }
}