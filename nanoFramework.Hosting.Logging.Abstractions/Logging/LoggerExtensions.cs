//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Hosting.Logging
{
    /// <summary>
    /// <see cref="ILogger"/>extension methods.
    /// </summary>
    public static class LoggerExtensions
    {
        private static readonly EventId _eventId = new EventId();

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="message">The entry to be written.</param>
        public static void Log(this ILogger logger, LogLevel logLevel, string message)
        {
            logger.Log(logLevel, _eventId, message, null);
        }

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="message">The entry to be written.</param>
        /// <param name="exception">The exception related to this entry.</param>
        public static void Log(this ILogger logger, LogLevel logLevel, string message, Exception exception)
        {
            logger.Log(logLevel, _eventId, message, exception);
        }
    }
}