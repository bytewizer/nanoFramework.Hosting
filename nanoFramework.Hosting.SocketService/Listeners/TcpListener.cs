//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Threading;
using System.Net.Sockets;

using nanoFramework.Hosting.Sockets.Channel;
using nanoFramework.Hosting.Sockets.Extensions;

namespace nanoFramework.Hosting.Sockets.Listener

{
    /// <summary>
    /// Represents an implementation of the <see cref="SocketListener"/> which listens for remote TCP clients.
    /// </summary>
    public class TcpListener : SocketListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TcpListener"/> class.
        /// <param name="options">Factory used to create objects used in this library.</param>
        /// </summary>
        public TcpListener(SocketListenerOptions options)
           : base(options)
        {
        }

        /// <summary>
        /// Accepted connection listening thread
        /// </summary>
        internal override void AcceptConnection()
        {
            int retry;

            // Signal the start method to continue
            _startedEvent.Set();

            while (Active)
            {
                retry = 0;

                try
                {
                    // Set the accept event to nonsignaled state
                    _acceptEvent.Reset();

                    // Waiting for a connection
                    var remoteSocket = _listenSocket.Accept();

                    var channel = new SocketChannel();

                    if (_options.IsTls)
                    {
                        channel.Assign(
                            remoteSocket,
                            _options.Certificate,
                            _options.SslProtocols);
                    }
                    else
                    {
                        channel.Assign(remoteSocket);
                    }

                    var thread = new Thread(() =>
                    {
                        // Signal the accept thread to continue
                        _acceptEvent.Set();

                        // Invoke the connected handler
                        OnConnected(channel);

                    });
                    thread.Priority = _options.ThreadPriority;
                    thread.Start();


                    //ThreadPool.QueueUserWorkItem(
                    //    new WaitCallback(delegate (object state)
                    //    {
                    //        // Signal the accept thread to continue
                    //        _acceptEvent.Set();

                    //        // Invoke the connected handler
                    //        OnConnected(channel);
                    //    }));

                    // Wait until a connection is made before continuing
                    _acceptEvent.WaitOne();
                }
                catch (SocketException ex)
                {
                    //The listen socket was closed
                    if (ex.IsIgnorableSocketException())
                    {
                        OnDisconnected(ex);
                        continue;
                    }

                    if (retry > _options.SocketRetry)
                    {
                        throw;
                    }

                    retry++;
                    continue;
                }
                catch (Exception ex)
                {
                    if (ex is ObjectDisposedException || ex is NullReferenceException)
                    {
                        break;
                    }

                    OnDisconnected(ex);

                    // Signal the accept thread to continue
                    _acceptEvent.Set();

                    // try again
                    continue;
                }

                Thread.Sleep(1);
            }
        }
    }
}
