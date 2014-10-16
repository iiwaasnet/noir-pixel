using System;
using System.Web.Http;
using Api.Logging.Models;
using Diagnostics;

namespace Api.Logging.Controllers
{
    [RoutePrefix("log")]
    public class LoggingController : ApiController
    {
        private readonly ILogger logger;

        public LoggingController(ILogger logger)
        {
            this.logger = logger;
        }

        [Route("add")]
        public void Add(ApplicationLoggingModel entry)
        {
            LogEntry(entry);
        }

        private void LogEntry(ApplicationLoggingModel entry)
        {
            try
            {
                switch (entry.Type.ToLower())
                {
                    case "debug":
                        logger.Debug(entry);
                        return;
                    case "warn":
                        logger.Warn(entry);
                        return;
                    case "exception":
                        logger.Error(entry);
                        return;
                    default:
                        logger.Warn(entry, "UNCATEGORIZED LOG ENTRY");
                        return;
                }
            }
            catch (Exception err)
            {
                logger.Error(err);
            }
        }
    }
}