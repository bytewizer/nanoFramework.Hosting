//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Net;

using nanoFramework.Hosting.Sockets.Listener;

namespace nanoFramework.Hosting.Sockets
{
    /// <summary>
    /// Represents configuration options of server specific features.
    /// </summary>
    public class ServerOptions : IServerOptions
    {
        /// <summary>
        /// Configuration options of socket specific features.
        /// </summary>
        internal SocketListenerOptions Listener { get; private set; } = new SocketListenerOptions();

        /// <inheritdoc/>
        public ServerLimits Limits { get; } = new ServerLimits();

        /// <inheritdoc/>
        public void Listen()
        {
            Listen(IPAddress.Any, 0);
        }

        /// <inheritdoc/>
        public void Listen(int port)
        {
            Listen(IPAddress.Any, port, _ => { });
        }

        /// <inheritdoc/>
        public void Listen(IPEndPoint endPoint)
        {
            Listen(endPoint, _ => { });
        }

        /// <inheritdoc/>
        public void Listen(IPAddress address, int port)
        {
            Listen(address, port, _ => { });
        }

        /// <inheritdoc/>
        public void Listen(int port, SocketListenerOptionsDelegate configure)
        {
            Listen(IPAddress.Any, port, configure);
        }

        /// <inheritdoc/>
        public void Listen(IPAddress address, int port, SocketListenerOptionsDelegate configure)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (port > IPEndPoint.MaxPort || port < IPEndPoint.MinPort)
            {
                throw new ArgumentOutOfRangeException(nameof(port), $"Must be less then { IPEndPoint.MaxPort } and greater then { IPEndPoint.MinPort }.");
            }

            Listen(new IPEndPoint(address, port), configure);
        }

        /// <inheritdoc/>
        public void Listen(IPEndPoint endPoint, SocketListenerOptionsDelegate configure)
        {
            if (endPoint == null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            Listener.EndPoint = endPoint;
            configure(Listener);
        }
    }
}