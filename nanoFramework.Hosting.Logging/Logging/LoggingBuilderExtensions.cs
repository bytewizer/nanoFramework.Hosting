//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using Microsoft.Extensions.Logging;
using System.Reflection;

namespace nanoFramework.Hosting.Logging
{
    /// <summary>
    /// Extension methods for setting up logging services in an <see cref="ILoggingBuilder" />.
    /// </summary>
    public static class LoggingBuilderExtensions
    {
        /// <summary>
        /// Sets a minimum <see cref="LogLevel"/> requirement for log messages to be logged.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to set the minimum level on.</param>
        /// <param name="level">The <see cref="LogLevel"/> to set as the minimum.</param>
        /// <returns>The <see cref="ILoggingBuilder"/> so that additional calls can be chained.</returns>
        public static ILoggingBuilder SetMinimumLevel(this ILoggingBuilder builder, LogLevel level)
        {

            return builder;
        }

        public static ILoggingBuilder SetMessageFormatter(this ILoggingBuilder builder)
        {
            builder.MessageFormatter(typeof(DefaultLoggerFormat).GetType().GetMethod("MessageFormatter"));

            return builder;
        }

        public static ILoggingBuilder MessageFormatter(this ILoggingBuilder builder, MethodInfo formatter)
        {
            LoggerExtensions.MessageFormatter = formatter;

            return builder;
        }
    }
}
