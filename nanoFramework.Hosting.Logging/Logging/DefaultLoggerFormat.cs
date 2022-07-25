using System;
using Microsoft.Extensions.Logging;


namespace nanoFramework.Hosting.Logging
{
    public class DefaultLoggerFormat
    {
        public string MessageFormatter(string className, LogLevel logLevel, EventId eventId, string state, Exception exception)
        {
            string logstr = string.Empty;
            switch (logLevel)
            {
                case LogLevel.Trace:
                    logstr = "TRACE: ";
                    break;
                case LogLevel.Debug:
                    logstr = "DEBUG: ";
                    break;
                case LogLevel.Warning:
                    logstr = "WARNING: ";
                    break;
                case LogLevel.Error:
                    logstr = "ERROR: ";
                    break;
                case LogLevel.Critical:
                    logstr = "CRITICAL:";
                    break;
                case LogLevel.Information:
                    logstr = "INFO: ";
                    break;
                case LogLevel.None:
                default:
                    break;
            }

            string eventstr = eventId.Id != 0 ? $"EVENT ID: {eventId}, " : string.Empty;
            string msg = $"[{className}] {eventstr}{logstr} {state}";
            if (exception != null)
            {
                msg += $" {exception}";
            }

            return msg;
        }
    }
}
