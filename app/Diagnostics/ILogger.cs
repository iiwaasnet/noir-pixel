namespace Diagnostics
{
    public interface ILogger
    {
        void Debug(string formatMessage, object obj);
        void Debug(object obj);
        void Error(string formatMessage, object obj);
        void Error(object obj);
        void Warn(string formatMessage, object obj);
        void Warn(object obj);
        void Info(string formatMessage, object obj);
        void Info(object obj);
    }
}