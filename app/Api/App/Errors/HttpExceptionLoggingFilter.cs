using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Antlr.Runtime.Misc;
using Api.App.Diagnostics.Config;
using Api.App.Errors.Extensions;
using Diagnostics;
using TypedConfigProvider;

namespace Api.App.Errors
{
    public class HttpExceptionLoggingFilter : ActionFilterAttribute
    {
        private readonly ILogger logger;
        private readonly LoggingConfiguration config;

        public HttpExceptionLoggingFilter(ILogger logger, IConfigProvider configProvider)
        {
            this.logger = logger;
            config = configProvider.GetConfiguration<DiagnosticsConfiguration>().Logging;
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            actionExecutedContext.ShieldException();

            if (ShouldLog(actionExecutedContext))
            {
                foreach (var contentConverter in ContentConverters())
                {
                    var httpError = contentConverter(actionExecutedContext.Response);
                    if (httpError != null)
                    {
                        logger.Error(httpError);

                        return;
                    }
                }
            }
        }

        private bool ShouldLog(HttpActionExecutedContext actionExecutedContext)
        {
            return actionExecutedContext.Response != null
                   &&
                   (config.LogAllHttpErrors && actionExecutedContext.Response.StatusCode >= HttpStatusCode.BadRequest
                    || actionExecutedContext.Response.StatusCode >= HttpStatusCode.InternalServerError);
        }

        private IEnumerable<Func<HttpResponseMessage, object>> ContentConverters()
        {
            yield return (response) => ObjectContentValue<ApiError>(response);
            yield return (response) => ObjectContentValue<ValidationError>(response);
            yield return (response) => ObjectContentValue(response);
            yield return (response) => response.ReasonPhrase;
        }

        private static object ObjectContentValue(HttpResponseMessage response)
        {
            var content = response.Content as ObjectContent;
            return (content != null) ? content.Value : null;
        }

        private static object ObjectContentValue<T>(HttpResponseMessage response)
        {
            var content = response.Content as ObjectContent<T>;
            return (content != null) ? content.Value : null;
        }
    }
}