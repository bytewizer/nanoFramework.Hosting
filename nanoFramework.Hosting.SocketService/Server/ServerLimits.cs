//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Hosting.Sockets
{
    /// <summary>
    /// Represents configuration limits of server specific features.
    /// </summary>
    public class ServerLimits
    {
        /// <summary>
        /// Gets the maximum size in bytes of the request message.
        /// </summary>
        public long MaxMessageSize { get; set; } = 8 * 1024;

        /// <summary>
        /// Gets the minimum size in bytes of the request message. 
        /// </summary>
        public long MinMessageSize { get; set; } = 0;
    }
}