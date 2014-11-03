using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Diagnostics;

namespace Api
{
    public class CustomExceptionLogger : ExceptionLogger
    {
        private readonly ILogger logger;

        public CustomExceptionLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public override Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            logger.Error(ToString(context));

            return null;
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