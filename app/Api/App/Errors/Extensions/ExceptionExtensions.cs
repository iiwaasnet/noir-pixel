using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Api.App.Exceptions;

namespace Api.App.Errors.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ShieldException(this HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext != null
                && actionExecutedContext.Exception != null)
            {
                if (actionExecutedContext.Exception.GetType() == typeof (NotFoundException))
                {
                    actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotFound);
                }
                if (actionExecutedContext.Exception.GetType() == typeof(InvalidPotoStateException))
                {
                    actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.MethodNotAllowed);
                }
                if (actionExecutedContext.Exception.GetType() == typeof (NotSupportedException))
                {
                    actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                }
            }
        }
    }
}