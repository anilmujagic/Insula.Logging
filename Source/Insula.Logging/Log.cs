using Insula.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insula.Logging
{
    public static class Log
    {
        #region LogEventBuilder

        public static Action<LogEventBuilder> LogEventInit { get; set; }

        public static LogEventBuilder Event(LogLevel level, string message, params object[] args)
        {
            var logEventBuilder = new LogEventBuilder(_logTarget, level, message, args);

            if (LogEventInit != null)
                LogEventInit(logEventBuilder);

            return logEventBuilder;
        }

        public static LogEventBuilder Critical(string message, params object[] args)
        {
            return Event(LogLevel.Critical, message, args);
        }

        public static LogEventBuilder Error(string message, params object[] args)
        {
            return Event(LogLevel.Error, message, args);
        }

        public static LogEventBuilder Warning(string message, params object[] args)
        {
            return Event(LogLevel.Warning, message, args);
        }

        public static LogEventBuilder Information(string message, params object[] args)
        {
            return Event(LogLevel.Information, message, args);
        }

        public static LogEventBuilder Debug(string message, params object[] args)
        {
            return Event(LogLevel.Debug, message, args);
        }

        public static LogEventBuilder Trace(string message, params object[] args)
        {
            return Event(LogLevel.Trace, message, args);
        }

        public static LogEventBuilder Exception(Exception exception, LogLevel level = LogLevel.Critical)
        {
            return Event(level, exception.Message).Data(exception);
        }

        #endregion


        #region LogTarget

        private static ILogTarget _logTarget;
        public static ILogTarget LogTarget
        {
            get
            {
                if (_logTarget == null)
                    _logTarget = new DefaultLogTarget();

                return _logTarget;
            }
            set
            {
                _logTarget = value;
            }
        }

        #endregion
    }
}
