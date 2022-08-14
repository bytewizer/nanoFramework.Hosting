//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting.Logging
{
    /// <summary>
    /// ILogger extension methods.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="message">The entry to be written.</param>
        public static void Log(this ILogger logger, LogLevel logLevel, string message)
        {
            logger.Log(logLevel, new EventId(), message, null);
        }
    }
}