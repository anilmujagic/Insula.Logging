using Insula.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insula.Logging
{
    public class LogEventBuilder
    {
        internal LogEventBuilder(ILogTarget logTarget, LogLevel level, string message, params object[] args)
        {
            if (logTarget == null)
                throw new ArgumentNullException("logTarget");

            _logTarget = logTarget;

            _logEvent = new LogEvent();

            var utcTime = DateTime.UtcNow;
            _logEvent.UtcTime = utcTime;
            _logEvent.LocalTime = utcTime.ToLocalTime();
            _logEvent.Level = level;

            _message = message;
            _messageParameters = args;
        }

        private readonly ILogTarget _logTarget;
        private LogEvent _logEvent;
        private string _message;
        private object[] _messageParameters;
        private object _data;

        private void ComposeLogEvent()
        {
            if (!_message.IsNullOrWhiteSpace())
                _logEvent.Message = _message.FormatString(_messageParameters);

            if (_data != null)
                _logEvent.Data = Newtonsoft.Json.JsonConvert.SerializeObject(_data);
        }


        #region Add optional info

        public LogEventBuilder Data(string data, LogDataType dataType = LogDataType.Text)
        {
            _data = null;
            _logEvent.Data = data;
            _logEvent.DataType = dataType;
            return this;
        }

        public LogEventBuilder Data(object data)
        {
            _data = data;
            _logEvent.DataType = LogDataType.Json;
            return this;
        }

        public LogEventBuilder Context(string value)
        {
            _logEvent.Context = value;
            return this;
        }

        public LogEventBuilder Reference(string value)
        {
            _logEvent.Reference = value;
            return this;
        }

        #endregion


        #region Add environment info

        public LogEventBuilder ApplicationVersion(string value)
        {
            _logEvent.ApplicationVersion = value;
            return this;
        }

        public LogEventBuilder Machine(string value)
        {
            _logEvent.Machine = value;
            return this;
        }

        public LogEventBuilder User(string value)
        {
            _logEvent.User = value;
            return this;
        }

        public LogEventBuilder Session(string value)
        {
            _logEvent.Session = value;
            return this;
        }

        #endregion


        public void Submit()
        {
            this.ComposeLogEvent();

            try
            {
                _logTarget.Submit(_logEvent);
            }
            catch
            {
#if DEBUG
                throw;
#endif
            }
        }
    }
}
