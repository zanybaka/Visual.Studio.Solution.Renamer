using System;
using Serilog;
using Serilog.Events;

namespace Visual.Studio.Solution.Renamer.Desktop.UI
{
    public class UILogger : ILogger
    {
        private readonly Action<string> _action;
        private readonly bool _verbose;

        public UILogger(Action<string> action, bool verbose)
        {
            _action  = action;
            _verbose = verbose;
        }

        public void Write(LogEvent logEvent)
        {
            if ((logEvent.Level == LogEventLevel.Verbose || logEvent.Level == LogEventLevel.Debug) && !_verbose)
            {
                return;
            }

            _action(logEvent.MessageTemplate.Text);
        }
    }
}