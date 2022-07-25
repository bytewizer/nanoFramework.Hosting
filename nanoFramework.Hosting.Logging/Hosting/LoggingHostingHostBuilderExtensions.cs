//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Hosting.Logging
{
    /// <summary>
    /// Extension methods for setting up logging in an <see cref="IHostBuilder" />.
    /// </summary>
    public static class LoggingHostingHostBuilderExtensions
    {
        /// <summary>
        /// Adds a delegate for configuring the provided <see cref="ILoggingBuilder"/>. This may be called multiple times.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
        /// <param name="configureLogging">The delegate that configures the <see cref="ILoggingBuilder"/>.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder, LoggingContextDelegate configureLogging)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder => configureLogging(context, builder)));
        }

        /// <summary>
        /// Adds a delegate for configuring the provided <see cref="ILoggingBuilder"/>. This may be called multiple times.
        /// </summary>
        /// <param name="hostBuilder">The <see cref="IHostBuilder" /> to configure.</param>
        /// <param name="configureLogging">The delegate that configures the <see cref="ILoggingBuilder"/>.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
        public static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder, LoggingAction configureLogging)
        {
            return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder => configureLogging(builder)));
        }
    }
}
