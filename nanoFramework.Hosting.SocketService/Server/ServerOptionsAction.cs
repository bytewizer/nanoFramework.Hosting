//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Hosting.Pipeline.Builder;

namespace nanoFramework.Hosting.Sockets
{
    /// <summary>
    /// Represents an options method to configure <see cref="SocketServerService"/> specific features.
    /// </summary>
    /// <param name="configure">The <see cref="SocketServerService"/> configuration specific features.</param>
    public delegate void ServerOptionsAction(IServerOptions configure);
}