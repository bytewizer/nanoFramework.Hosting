//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Hosting.Logging;
using nanoFramework.Hosting.Sockets.Channel;
using nanoFramework.Hosting.Sockets.Listener;
using System;
using System.Net.Sockets;

namespace nanoFramework.Hosting.Sockets
{
    /// <summary>
    /// Represents a base implementation of <see cref="SocketService"/> which uses <see cref="SocketListener"/> for serving requests.
    /// </summary>
    public abstract class SocketService : IHostedService
    {
        /// <summary>
        /// The logger used to write messages.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The <see cref="SocketListener"/> which listens for remote clients.
        /// </summary>
        protected SocketListener _listener;

        /// <summary>
        /// The logger factory used to write to.
        /// </summary>
        protected ILoggerFactory _loggerFactory;

        /// <summary>
        /// The configuration options of server specific features.
        /// </summary>
        protected ServerOptions _options = new ServerOptions();

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketService"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        public SocketService(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger(nameof(SocketService));
        }

        /// <summary>
        /// Set <see cref="SocketListener"/> based on <see cref="ProtocolType"/>.
        /// </summary>
        public void SetListener()
        {
            switch (_options.Listener.ProtocolType)
            {
                case ProtocolType.Tcp:
                    _listener = new TcpListener(_options.Listener);
                    break;
                case ProtocolType.Udp:
                    _listener = new UdpListener(_options.Listener);
                    break;
                default:
                    throw new NotSupportedException();
            }

            _listener.Connected += ClientConnected;
            _listener.Disconnected += ClientDisconnected;
        }

        ///<inheritdoc/>
        public void Start()
        {
            try
            {
                var status = _listener.Start();
                _logger.StartingService(_listener.ActivePort);

                //return status;
            }
            catch (Exception ex)
            {
                _logger.StartingService(_listener.ActivePort, ex);
                //return false;
            }
        }

        ///<inheritdoc/>
        public void Stop()
        {
            try
            {
                var status = _listener.Stop();
                _logger.StoppingService(_listener.ActivePort);

                //return status;
            }
            catch (Exception ex)
            {
                _logger.StoppingService(_listener.ActivePort, ex);
                //return false;
            }
        }

        /// <summary>
        /// A client has connected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="channel">The socket channel for the connected endpoint.</param>
        protected virtual void ClientConnected(object sender, SocketChannel channel) { }

        /// <summary>
        /// A client has disconnected.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="execption">The socket <see cref="Exception"/> for the disconnected endpoint.</param>
        protected virtual void ClientDisconnected(object sender, Exception execption) { }
    }
}