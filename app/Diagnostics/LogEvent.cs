using System;

namespace Diagnostics
{
    public class LogEvent
    {
        public Exception Error { get; set; }
        public string Message { get; set; }
        public EventId EventId { get; set; }

        public override string ToString()
        {
            return string.Format("EventId:[{0}], Message:[{1}], Error:[{2}]", EventId, Error, Message);
        }
    }
}