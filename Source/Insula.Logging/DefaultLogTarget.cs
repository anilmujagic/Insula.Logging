using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insula.Logging
{
    public class DefaultLogTarget : ILogTarget
    {
        public void Submit(LogEvent logEvent)
        {
            System.Diagnostics.Debug.WriteLine("[{0}] {1}", logEvent.Level, logEvent.Message);
        }
    }
}
