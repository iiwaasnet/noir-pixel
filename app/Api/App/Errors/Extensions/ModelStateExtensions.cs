using System.Web.Http.ModelBinding;

namespace Api.App.Errors.Extensions
{
    public static class ModelStateExtensions
    {
        public static ValidationError ToValidationError(this ModelStateDictionary modelState)
        {
        }
    }
}