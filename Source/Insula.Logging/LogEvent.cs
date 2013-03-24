using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insula.Logging
{
    public class LogEvent
    {
        // Mandatory
        public DateTime UtcTime { get; set; }
        public DateTime LocalTime { get; set; }
        public LogLevel Level { get; set; }
        public string Message { get; set; }

        // Optional
        public string Data { get; set; }
        public LogDataType DataType { get; set; }
        public string Context { get; set; }
        public string Reference { get; set; }

        // Should be populated by application code in LogInit event
        public string ApplicationVersion { get; set; }
        public string Machine { get; set; }
        public string User { get; set; }
        public string Session { get; set; }
    }
}
