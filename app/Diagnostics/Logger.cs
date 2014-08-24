using NLog;

namespace Diagnostics
{
    public class Logger : ILogger
    {
        private readonly NLog.Logger logger;
        public Logger()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public Logger(string name)
        {
            logger = LogManager.GetLogger(name);
        }

        public void Debug(object obj, string message)
        {
            logger.Debug(message, obj);
        }

        public void Debug(object obj)
        {
            logger.Debug(obj);
        }

        public void Error(object obj, string message)
        {
            logger.Error(message, obj);
        }

        public void Error(object obj)
        {
            logger.Error(obj);
        }

        public void Warn(object obj, string message)
        {
            logger.Warn(message, obj);
        }

        public void Warn(object obj)
        {
            logger.Warn(obj);
        }

        public void Info(object obj, string message)
        {
            logger.Info(message, obj);
        }

        public void Info(object obj)
        {
            logger.Info(obj);
        }
    }
}