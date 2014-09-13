namespace Api.Logging.Models
{
    public class ApplicationLoggingModel
    {
        public string Url { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string[] StackTrace { get; set; }
        public string Cause { get; set; }

        public override string ToString()
        {
            return string.Format("<Url>: {0} <Message>: {1} <Cause>: {2} <StackTrace>: {3}",
                          Url,
                          Message,
                          Cause,
                          (StackTrace != null) ? string.Join(" ", StackTrace) : "");
        }
    }
}