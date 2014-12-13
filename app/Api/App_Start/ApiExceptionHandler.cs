using System;
using System.Net;
using System.Net.Http.Formatting;
using System.Web.Http.ExceptionHandling;
using Api.App;

namespace Api
{
    public class ApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var contentNegotiator = (IContentNegotiator) context.RequestContext.Configuration.Services.GetService(typeof (IContentNegotiator));
            var formatters = context.RequestContext.Configuration.Formatters;

            context.Result = new ApiErrorResult<Exception>(HttpStatusCode.InternalServerError,
                                                           context.Exception,
                                                           new HttpContextDependencyProvider
                                                           {
                                                               ContentNegotiator = contentNegotiator,
                                                               Formatters = formatters,
                                                               Request = context.Request
                                                           });
        }
    }
}