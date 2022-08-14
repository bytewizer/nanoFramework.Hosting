//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;

namespace nanoFramework.Hosting.Logging
{
    internal class Logger : ILogger
    {
        public LoggerInformation[] Loggers { get; set; }

        public MessageLogger[] MessageLoggers { get; set; }

        public void Log(LogLevel logLevel, EventId eventId, object state, Exception exception)
        {
            MessageLogger[] loggers = MessageLoggers;
            
            if (loggers == null)
            {
                return;
            }

            ArrayList exceptions = null;

            for (int index = 0; index < loggers.Length; index++)
            {
                ref readonly MessageLogger loggerInfo = ref loggers[index];
                if (!loggerInfo.IsEnabled(logLevel))
                {
                    continue;
                }

                LoggerLog(logLevel, eventId, loggerInfo.Logger, exception, state, ref exceptions);
            }

            if (exceptions != null && exceptions.Count > 0)
            {
                throw new AggregateException(string.Empty, exceptions);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            MessageLogger[] loggers = MessageLoggers;

            if (loggers == null)
            {
                return false;
            }

            ArrayList exceptions = null;

            int i = 0;
            for (; i < loggers.Length; i++)
            {
                ref readonly MessageLogger loggerInfo = ref loggers[i];
                if (!loggerInfo.IsEnabled(logLevel))
                {
                    continue;
                }

                if (LoggerIsEnabled(logLevel, loggerInfo.Logger, ref exceptions))
                {
                    break;
                }
            }

            if (exceptions != null && exceptions.Count > 0)
            {
                throw new AggregateException(string.Empty, exceptions);
            }

            return i < loggers.Length;
        }

        private static void LoggerLog(LogLevel logLevel, EventId eventId, ILogger logger, Exception exception, object state, ref ArrayList exceptions)
        {
            if (logger == null)
            {
                return;
            }

            try
            {
                logger.Log(logLevel, eventId, state, exception);
            }
            catch (Exception ex)
            {
                if (exceptions == null)
                {
                    exceptions = new ArrayList();
                }

                exceptions.Add(ex);
            }
        }

        private static bool LoggerIsEnabled(LogLevel logLevel, ILogger logger, ref ArrayList exceptions)
        {
            try
            {
                if (logger.IsEnabled(logLevel))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                exceptions ??= new ArrayList();
                exceptions.Add(ex);
            }

            return false;
        }
    }
}