using Insula.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insula.Logging
{
    public class Log
    {
        private Log(LogLevel level, string message, params object[] args)
        {
            _logEvent = new LogEvent();

            var utcTime = DateTime.UtcNow;
            _logEvent.UtcTime = utcTime;
            _logEvent.LocalTime = utcTime.ToLocalTime();
            _logEvent.Level = level;

            _message = message;
            _messageParameters = args;
        }

        private LogEvent _logEvent;

        private string _message;
        private object[] _messageParameters;
        private object _data;

        public static Action<Log> LogInit { get; set; }

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


        #region Instantiate

        public static Log Event(LogLevel level, string message, params object[] args)
        {
            var log = new Log(level, message, args);

            if (LogInit != null)
                LogInit(log);

            return log;
        }

        public static Log Critical(string message, params object[] args)
        {
            return Event(LogLevel.Critical, message, args);
        }

        public static Log Error(string message, params object[] args)
        {
            return Event(LogLevel.Error, message, args);
        }

        public static Log Warning(string message, params object[] args)
        {
            return Event(LogLevel.Warning, message, args);
        }

        public static Log Information(string message, params object[] args)
        {
            return Event(LogLevel.Information, message, args);
        }

        public static Log Debug(string message, params object[] args)
        {
            return Event(LogLevel.Debug, message, args);
        }

        public static Log Trace(string message, params object[] args)
        {
            return Event(LogLevel.Trace, message, args);
        }

        public static Log Exception(Exception exception, LogLevel level = LogLevel.Critical)
        {
            return Event(level, exception.Message).Data(exception);
        }

        #endregion


        #region Add optional info

        public Log Data(string data, LogDataType dataType = LogDataType.Text)
        {
            _data = null;
            _logEvent.Data = data;
            _logEvent.DataType = dataType;
            return this;
        }

        public Log Data(object data)
        {
            _data = data;
            _logEvent.DataType = LogDataType.Json;
            return this;
        }

        public Log Context(string value)
        {
            _logEvent.Context = value;
            return this;
        }

        public Log Reference(string value)
        {
            _logEvent.Reference = value;
            return this;
        }

        #endregion


        #region Add environment info

        public Log ApplicationVersion(string value)
        {
            _logEvent.ApplicationVersion = value;
            return this;
        }

        public Log Machine(string value)
        {
            _logEvent.Machine = value;
            return this;
        }

        public Log User(string value)
        {
            _logEvent.User = value;
            return this;
        }

        public Log Session(string value)
        {
            _logEvent.Session = value;
            return this;
        }

        #endregion


        #region Finalize and send

        private void PopulateFields()
        {
            if (!_message.IsNullOrWhiteSpace())
                _logEvent.Message = _message.FormatString(_messageParameters);

            if (_data != null)
                _logEvent.Data = Newtonsoft.Json.JsonConvert.SerializeObject(_data);
        }

        public void Submit()
        {
            this.PopulateFields();

            try
            {
                LogTarget.Submit(_logEvent);
            }
            catch
            {
#if DEBUG
                throw;
#endif
            }
        }

        #endregion
    }
}
