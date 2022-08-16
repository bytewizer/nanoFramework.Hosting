//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Hosting.Logging;
using nanoFramework.Hosting.Sockets;
using nanoFramework.Hosting.Pipeline;
using nanoFramework.Hosting.Sockets.Channel;
using nanoFramework.Hosting.Pipeline.Builder;
using nanoFramework.Hosting.Sockets.Listener;
using System;

namespace nanoFramework.Hosting
{

    /// <summary>
    /// Represents an implementation of the <see cref="SocketServerService"/> for creating network servers.
    /// </summary>
    public class SocketServerService : SocketService, IHostedService
    {
        private readonly ILogger _logger;
        private readonly IApplication _application;

        private readonly ContextPool _contextPool;

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketServerService"/> class.
        /// </summary>
        /// <param name="services">The service provider.</param>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The configuration options of <see cref="SocketServerService"/> specific features.</param>
        public SocketServerService(IServiceProvider services, ILoggerFactory loggerFactory, IServerOptions options)
            : base (loggerFactory)
        {
            var applicationBuilder = (IApplicationBuilder)services.GetService(typeof(ISocketServiceBuilder));
            
            if (applicationBuilder == null)
            {
                applicationBuilder = new ApplicationBuilder(services);
                applicationBuilder.Use((context, next) => { });
            }
            
            _application = applicationBuilder.Build();

            _logger = loggerFactory.CreateLogger(nameof(SocketServerService));
            _contextPool = new ContextPool(services);
            _options = options as ServerOptions;
            
            SetListener();
        }

        /// <summary>
        /// Gets configuration options of socket specific features.
        /// </summary>
        public SocketListenerOptions ListenerOptions { get => _listener?.Options; }

        /// <summary>
        /// Gets port that the server is actively listening on.
        /// </summary>
        public int ActivePort { get => _listener.ActivePort; }

        /// <inheritdoc />
        void IHostedService.Start()
        {
            Start();
        }

        /// <inheritdoc />
        void IHostedService.Stop()
        {
            Stop();
        }

        /// <inheritdoc />
        protected override void ClientConnected(object sender, SocketChannel channel)
        {
            // Set channel error handler
            channel.ChannelError += ChannelError;

            try
            {
                // Get context from context pool
                var context = _contextPool.GetContext(typeof(SocketContext)) as SocketContext;

                // Assign channel
                context.Channel = channel;

                // invoke pipeline 
                _application.Invoke(context);

                // Release context back to pool and close connection once pipeline is complete
                _contextPool.Release(context);
            }
            catch (Exception ex)
            {
                _logger.UnhandledException(ex);
                return;
            }
        }

        /// <inheritdoc />
        protected override void ClientDisconnected(object sender, Exception execption)
        {
            _logger.RemoteDisconnect(execption);
        }

        /// <inheritdoc />
        private void ChannelError(object sender, Exception execption)
        {
            _logger.ChannelExecption(execption);
        }
    }
}