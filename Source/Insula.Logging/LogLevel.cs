using System;

namespace Insula.Logging
{
    public enum LogLevel : byte
    {
        Critical = 1,
        Error,
        Warning,
        Information,
        Debug,
        Trace
    }
}