//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;

using nanoFramework.Hosting.Logging;
using nanoFramework.Hosting.Sockets.Channel;

namespace nanoFramework.Hosting.Sockets
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class DiagnosticsLoggerExtensions
    {
        public static void UnhandledException(this ILogger logger, string name, string message, Exception exception)
        {
            logger.Log(
                LogLevel.Error,
                message,
                exception
                );
        }

        public static void UnhandledException(this ILogger logger, Exception exception)
        {
            logger.Log(
                LogLevel.Error,
                "An unhandled exception has occurred while executing.",
                exception
                );
        }

        public static void ServiceExecption(this ILogger logger, string message, Exception exception)
        {
            logger.Log(
                LogLevel.Information,
                message,
                exception
                );
        }

        public static void StartingService(this ILogger logger, int port)
        {
            var message = $"Started socket listener bound to port {port}.";

            if (logger.GetType() == typeof(NullLogger))
            {
                Debug.WriteLine(message);
            }

            logger.Log(
                LogLevel.Information,
                message
                );
        }

        public static void StartingService(this ILogger logger, int port, Exception exception)
        {
            logger.Log(
                LogLevel.Error,
                $"Error starting socket listener bound to port {port}.",
                exception
                );
        }

        public static void StoppingService(this ILogger logger, int port)
        {
            var message = $"Stopping socket listener bound to port {port}.";
            
            if (logger.GetType() == typeof(NullLogger))
            {
                Debug.WriteLine(message);
            }

            logger.Log(
                LogLevel.Information,
                message
                );
        }

        public static void StoppingService(this ILogger logger, int port, Exception exception)
        {
            logger.Log(
                LogLevel.Error,
                $"Error stopping socket listener bound to port {port}.",
                exception
                );
        }

        public static void RemoteConnected(this ILogger logger, SocketChannel channel)
        {
            logger.Log(
                LogLevel.Debug,
                $"Remote client {channel.Connection.RemoteIpAddress}:{channel.Connection.RemotePort} has connected."
                );
        }

        public static void RemoteClosed(this ILogger logger, SocketChannel channel)
        {
            logger.Log(
                LogLevel.Debug,
                $"Remote client {channel.Connection.RemoteIpAddress}:{channel.Connection.RemotePort} has closed connection."
                );
        }

        public static void RemoteDisconnect(this ILogger logger, Exception exception)
        {
            logger.Log(
                LogLevel.Debug,
                $"Remote client has disconnected connection.",
                exception
                );
        }

        public static void ChannelExecption(this ILogger logger, Exception exception)
        {
            logger.Log(
                LogLevel.Error,
                $"Unexpcted channel exception occured.",
                exception
                );
        }

        public static void InvalidMessageLimit(this ILogger logger, long length, long min, long max)
        {
            logger.Log(
                LogLevel.Debug,
                $"Invalid message limit length:{length} min:{min} max:{max}.",
                null
                );
        }

        public static void SocketTransport(this ILogger logger, string message)
        {
            logger.Log(
                LogLevel.Trace,
                message
                );
        }
    }
}
