//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Hosting.Pipeline;
using nanoFramework.Hosting.Sockets.Channel;

namespace nanoFramework.Hosting.Sockets
{
    /// <summary>
    /// An interface for <see cref="SocketContext"/>.
    /// </summary>
    public interface ISocketContext : IContext
    {
        /// <summary>
        /// Gets or sets information about the underlying connection for this request.
        /// </summary>
        SocketChannel Channel { get; set; }
    }
}