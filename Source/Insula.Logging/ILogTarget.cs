using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Insula.Logging
{
    public interface ILogTarget
    {
        void Submit(LogEvent logEvent);
    }
}
