//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting.Sockets.Listener
{
    /// <summary>
    /// Represents an options method to configure <see cref="SocketListener"/> specific features.
    /// </summary>
    /// <param name="configure">The <see cref="SocketListener"/> configuration specific features.</param>
    public delegate void SocketListenerOptionsDelegate(SocketListenerOptions configure);
}