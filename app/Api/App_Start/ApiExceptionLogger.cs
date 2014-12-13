using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Diagnostics;

namespace Api
{
    public class ApiExceptionLogger : ExceptionLogger
    {
        private readonly ILogger logger;

        public ApiExceptionLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            logger.Error(ToString(context));
        }

        private static string ToString(ExceptionLoggerContext context)
        {
            return string.Format("{0} {1} Url:{2}",
                                 context.Exception.ToString(),
                                 context.Request.Method.Method,
                                 context.Request.RequestUri.AbsoluteUri);
        }
    }
}