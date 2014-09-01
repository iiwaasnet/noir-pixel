﻿namespace Api.Logging.Models
{
    public class ErrorLoggingModel
    {
        public string Url { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string[] StackTrace { get; set; }
        public string Cause { get; set; }
    }
}