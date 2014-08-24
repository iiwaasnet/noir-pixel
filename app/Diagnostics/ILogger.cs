namespace Diagnostics
{
    public interface ILogger
    {
        void Debug(object obj, string message);
        void Debug(object obj);
        void Error(object obj, string message);
        void Error(object obj);
        void Warn(object obj, string message);
        void Warn(object obj);
        void Info(object obj, string message);
        void Info(object obj);
    }
}