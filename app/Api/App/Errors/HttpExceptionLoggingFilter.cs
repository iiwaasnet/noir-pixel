using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Antlr.Runtime.Misc;
using Api.App.Diagnostics.Config;
using Diagnostics;
using JsonConfigurationProvider;

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

            if (config.LogAllHttpErrors
                && actionExecutedContext.Response != null
                && actionExecutedContext.Response.StatusCode >= HttpStatusCode.BadRequest)
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

        private IEnumerable<Func<HttpResponseMessage, object>> ContentConverters()
        {
            yield return (response) => response.Content as ObjectContent<ApiError>;
            yield return (response) => response.Content as ObjectContent<ValidationError>;
            yield return (response) => response.Content as ObjectContent;
            yield return (response) => response.ReasonPhrase;
        }
    }
}