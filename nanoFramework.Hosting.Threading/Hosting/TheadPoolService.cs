//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Hosting.Logging;
using System;
using System.Threading;

namespace nanoFramework.Hosting.Threading
{
    /// <summary>
    /// Defines methods for thread pool services managed by the host.
    /// </summary>
    public class TheadPoolService : IHostedService
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TheadPoolService"/> class.
        /// </summary>
        public TheadPoolService()
            : this(NullLoggerFactory.Instance)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TheadPoolService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        public TheadPoolService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(TheadPoolService));
        }

        /// <inheritdoc/>
        public void Start()
        {
            ThreadPool.UnhandledThreadPoolException += ThreadUnhandledThreadPoolException;
        }

        /// <inheritdoc/>
        public void Stop()
        {
            ThreadPool.UnhandledThreadPoolException -= ThreadUnhandledThreadPoolException;
            ThreadPool.Shutdown();
        }

        private void ThreadUnhandledThreadPoolException(object state, Exception ex)
        {
            _logger.Log(LogLevel.Error, "Unhandled thread pool exception.", ex);
        }
    }
}