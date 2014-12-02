using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NLog;

namespace Diagnostics
{
    public class Logger : ILogger
    {
        private readonly NLog.Logger logger;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        public Logger()
            : this(LogManager.GetCurrentClassLogger())
        {
        }

        public Logger(string name)
            : this(LogManager.GetLogger(name))
        {
        }

        private Logger(NLog.Logger logger)
        {
            this.logger = logger;
            jsonSerializerSettings = new JsonSerializerSettings
                                     {
                                         ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                         Converters = {new JavaScriptDateTimeConverter()}
                                     };
        }

        public void Debug(string formatMessage, object obj)
        {
            logger.Debug(formatMessage, JsonConvert.SerializeObject(obj, jsonSerializerSettings));
        }

        public void Debug(object obj)
        {
            logger.Debug(JsonConvert.SerializeObject(obj, jsonSerializerSettings));
        }

        public void Error(string formatMessage, object obj)
        {
            logger.Error(formatMessage, JsonConvert.SerializeObject(obj, jsonSerializerSettings));
        }

        public void Error(object obj)
        {
            logger.Error(JsonConvert.SerializeObject(obj, jsonSerializerSettings));
        }

        public void Warn(string formatMessage, object obj)
        {
            logger.Warn(formatMessage, JsonConvert.SerializeObject(obj, jsonSerializerSettings));
        }

        public void Warn(object obj)
        {
            logger.Warn(JsonConvert.SerializeObject(obj, jsonSerializerSettings));
        }

        public void Info(string formatMessage, object obj)
        {
            logger.Info(formatMessage, JsonConvert.SerializeObject(obj, jsonSerializerSettings));
        }

        public void Info(object obj)
        {
            logger.Info(JsonConvert.SerializeObject(obj, jsonSerializerSettings));
        }
    }
}